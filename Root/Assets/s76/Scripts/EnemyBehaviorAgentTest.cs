using UnityEngine;
using System.Collections;

public class EnemyBehaviorAgentTest : MonoBehaviour
{
	public Transform target;
	public Transform[] patrol_points;
	public float patrol_point_displacement_strength=1;
	public float stay_time=2;

	EnemyBehaviorAgent e_agent;
	void Awake() {
		e_agent = GetComponent<EnemyBehaviorAgent>();
	}

	void Start () {
		e_agent.SetPatrolPath(patrol_points,true, 1);
	}

	void Update () {
		if ( e_agent.CanSeeTarget(target ) ) {
			Debug.Log("Detected target - gonna chase him down !");
			e_agent.Chase(target);
		} else {
			//Debug.Log("See nothing - must do my work !");
			e_agent.Patrol(stay_time);
		}
	}
	void OnGUI (){
		if ( GUILayout.Button("Force Stop Chase" )) {
			e_agent.Patrol(stay_time);
		}
	}

}

