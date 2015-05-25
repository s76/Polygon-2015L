using UnityEngine;
using System.Collections;

public class TEST_NavMeshMove : MonoBehaviour {
	public Vector3 destination;
	NavMeshAgent agent;

	// Use this for initialization
	void Start () {
		agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {
		//agent.SetDestination(destination);
	}

	void OnGUI () {
		if( GUILayout.Button("STOP")) {
			agent.Stop();
		}
		if( GUILayout.Button("RESUME")) {
			agent.Resume();
		}
		if( GUILayout.Button("WAPS")) {
			agent.Warp(destination);
		}
	}
}
