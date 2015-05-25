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

	void Awake () {
		agent = GetComponent<NavMeshAgent>();
		trans = GetComponent<Transform>();
	}

	void OnDrawGizmos () {
		Gizmos.color = Color.white;
		Gizmos.DrawLine(transform.position,transform.position + transform.forward*vision_range);

		if ( p_points != null ) {
			Gizmos.color = Color.yellow;
			for( int i =0;i < p_points.Length; i++ ) {
				var next = i + 1 < p_points.Length ? i+ 1 : i+1 - p_points.Length;
				Gizmos.DrawLine(p_points[i], p_points[next]);
			}
		}
	}
	
	Vector3[] p_points;
	int last_patrol_point=-1;
	float stay_time;

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

	Transform chase_target;
	public void Chase ( Transform target ) {
		chase_target = target;
		current_state = State.Chase;
	}

	
	[SerializeField]
	float stay_rotation_speed;
	float stay_timer;
	Quaternion last_rotation;
	float current_break_angle;

	void Update () {
		if ( current_state == State.None ) {}
		else if ( current_state == State.Patrol ) {
			if ( (p_points[last_patrol_point] - trans.position).sqrMagnitude < 1f ) {
				current_state = State.Stay;
				stay_timer = 0;
				last_rotation = trans.rotation;
				current_break_angle = Random.value*Random.value* 180;
			}
			agent.SetDestination(p_points[last_patrol_point]);
		}
		else if ( current_state == State.Stay ) {
			if ( stay_timer > stay_time ) {
				current_state = State.Patrol;
				last_patrol_point += 1;
				if ( last_patrol_point >= p_points.Length ) last_patrol_point -= p_points.Length;
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

