using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Path : MonoBehaviour {
	public Train train;
	public Transform[] points;
	public int resolution;
	public float progress_step;

	Vector3[] genPath;
	bool moving;

	void OnGUI () {
		if ( GUILayout.Button("Analize") ) {
			Analize(points,resolution,progress_step);
		}
		if ( GUILayout.Button("Gen Path " )) {
			genPath = f (points,resolution,progress_step);
		}

		if ( GUILayout.Button("Move ") ) {
			Debug.Log("Start moving");
			train.Prepare(genPath);
			moving = true;
		}
	}

	void Update () {
		if ( moving ) {
			moving = train.MovePerFrame(genPath,Time.deltaTime);
			if( !moving) Debug.Log("Finish moving");
		}
	}

	void OnDrawGizmos () {
		iTween.DrawPath(points,Color.green);
	}
	
	void Analize (Transform[] _control_points, int _resolution, float _progress_step) {
		float path_length = iTween.PathLength(_control_points);
		float fragment_length = path_length/_resolution;
		float min=float.MaxValue,max=float.MinValue,avg=0;
		Vector3 prev = _control_points[0].position;
		float progress;
		for(progress = _progress_step; progress <= 1; progress += _progress_step ) {
			Vector3 k = iTween.PointOnPath(_control_points,(progress));
			float t = (k-prev ).magnitude;
			avg += t;
			if ( t < min ) min = t;
			if ( t > max ) max = t;
			prev = k;
		}
		Debug.Log("fragment_length = "+ fragment_length);
		Debug.Log("max step len = " + max);
		Debug.Log("avg step len = " + avg/(1/_progress_step));
		Debug.Log("NOTE: \nthe greater resolution , the smaller fragment_length");
		Debug.Log("NOTE: \nthe smaller fragment_length , the better movement quality");
		Debug.Log("NOTE: \nthe larger fragment_length/max_step , the more accurate result" );
	}

	Vector3[] f (Transform[] _control_points, int _resolution, float _progress_step) {
		float path_length = iTween.PathLength(_control_points);
		float fragment_length = path_length/_resolution;

		List<Vector3> result = new List<Vector3>();
		result.Add(_control_points[0].position);

		float progress;
		int last_result = 0;

		for(progress = 0; progress <= 1; progress += _progress_step ) {
			Vector3 k = iTween.PointOnPath(_control_points,(progress));
			if( ( k - result[last_result]).magnitude > fragment_length ) {
				result.Add(k);
				last_result++;
			}
		}
		Debug.Log("Generated path with points nb = " + result.Count);
		return result.ToArray();
	}
}
