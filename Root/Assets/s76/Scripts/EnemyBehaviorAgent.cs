using UnityEngine;
using System.Collections;

[RequireComponent(typeof(NavMeshAgent))]
public class EnemyBehaviorAgent : MonoBehaviour
{
	public float vision_range;
	public float vision_angle;

	enum State { None, Patrol, Stay, Chase };

	State current_state;
	NavMeshAgent agent;
	Transform trans;

	Vector3[] p_points;
	int last_patrol_point=-1;
	float stay_time;
		
	Transform chase_target;
	[SerializeField]
	float stay_rotation_speed;
	float stay_timer;
	Quaternion last_rotation;
	float current_break_angle;
	
	float patrol_unstack_timer=0;
	float patrol_stack_time = 5f;
	bool start_stack_aware;

	void Awake () {
		agent = GetComponent<NavMeshAgent>();
		trans = GetComponent<Transform>();
	}
	
	Vector3 GizmoAngleFunction (float angle) {

		return new Vector3(-Mathf.Cos((angle+90)*Mathf.Deg2Rad),0,Mathf.Sin((angle+90)*Mathf.Deg2Rad));
	}

	void OnDrawGizmos () {
		Gizmos.color = Color.white;
		float y = transform.rotation.eulerAngles.y;
		Vector3 gizmo_vision_angle_left = GizmoAngleFunction( y - vision_angle/2f)* vision_range;
		Vector3 gizmo_vision_angle_right = GizmoAngleFunction(y + vision_angle/2f)* vision_range;
		Vector3 gizmo_vision_range = transform.forward*vision_range;

		Gizmos.DrawLine(transform.position,transform.position + gizmo_vision_range);
		Gizmos.DrawLine(transform.position,transform.position + gizmo_vision_angle_left);
		Gizmos.DrawLine(transform.position,transform.position + gizmo_vision_angle_right);
		Gizmos.DrawLine(transform.position + gizmo_vision_range,transform.position + gizmo_vision_angle_right);
		Gizmos.DrawLine(transform.position + gizmo_vision_range,transform.position + gizmo_vision_angle_left);

		if ( p_points != null ) {
			Gizmos.color = Color.yellow;
			for( int i =0;i < p_points.Length; i++ ) {
				var next = i + 1 < p_points.Length ? i+ 1 : i+1 - p_points.Length;
				Gizmos.DrawLine(p_points[i], p_points[next]);
			}
		}
	}

	public void SetPatrolPath ( Transform[] patrol_points, bool random_displacement, float displacement_length) {
		p_points = new Vector3[patrol_points.Length];
		float min_len = float.MaxValue;
		int closest_point = -1;
		for(int i=0;i < patrol_points.Length; i++ ) {
			p_points[i] = patrol_points[i].position;
			if ( random_displacement ) {
				var displacement = new Vector3(Random.value, 0, Random.value ) * displacement_length;
				p_points[i] += displacement;
			}
			var k = (p_points[i]-trans.position ).magnitude;
			if ( k  < min_len ) {
				min_len = k;
				closest_point = i;
			}
		}
		last_patrol_point = closest_point;
	}

	public void Patrol ( float stay_time ) {
		this.stay_time = stay_time;
		if( current_state != State.Stay && current_state != State.Patrol) current_state = State.Patrol;
	}

	public bool CanSeeTarget (Transform target ) {
		if ( (trans.position - target.position).magnitude > vision_range ) return false;
		if ( Vector3.Angle(target.position - trans.position,trans.forward) > vision_angle/2 ) return false;
		RaycastHit hit;
		if ( Physics.Raycast(trans.position,target.position - trans.position, out hit, vision_range ) ) {
			if ( hit.transform.position == target.position ) return true;
		}
		return false;
	}

	public void Chase ( Transform target ) {
		chase_target = target;
		current_state = State.Chase;
	}

	void Update () {
		if ( current_state == State.None ) {}
		else if ( current_state == State.Patrol ) {
			if ( agent.remainingDistance < 3 ) start_stack_aware = true;
			if ( start_stack_aware ) {
				patrol_unstack_timer += Time.deltaTime;
			}
			if ( patrol_unstack_timer > patrol_stack_time | agent.remainingDistance < 0.3f ) {
				current_state = State.Stay;
				stay_timer = agent.remainingDistance < 0.3f? 0: patrol_stack_time/2;
				patrol_unstack_timer = 0;
				start_stack_aware = agent.remainingDistance < 0.3f ? false: true;
				last_rotation = trans.rotation;
				current_break_angle = Random.value*Random.value* 180;
			}
		}
		else if ( current_state == State.Stay ) {
			if ( stay_timer > stay_time ) {
				current_state = State.Patrol;
				last_patrol_point += 1;
				if ( last_patrol_point >= p_points.Length ) last_patrol_point -= p_points.Length;
				agent.SetDestination(p_points[last_patrol_point]);
			} else {
				if ( Quaternion.Angle(trans.rotation, last_rotation ) < current_break_angle ) {
					trans.RotateAround( trans.position, Vector3.up, stay_rotation_speed*Time.deltaTime);
				} else {
					last_rotation = trans.rotation;
					current_break_angle = Random.value*Random.value* 180;
					stay_rotation_speed *= -1;
				}
				stay_timer  += Time.deltaTime;
			}
		}
		else if ( current_state == State.Chase ) {
			agent.SetDestination(chase_target.position);
		}
		else throw new UnityException("wtf ? ");
	}
}

