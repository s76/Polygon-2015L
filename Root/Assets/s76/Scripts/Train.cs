using UnityEngine;
using System.Collections;

public class Train : MonoBehaviour
{
	public MovableOnPath[] parts;
	public float speed;
	public float offset;
	bool stop_move; 

	public void ReplaceParts () { 
		for(int i =1; i < parts.Length; i++ ) {
			var p = parts[i-1].transform.position;
			parts[i].transform.position = new Vector3(p.x, p.y,p.z - offset);
		}
	}
	
	public void Prepare (Vector3[] path) {
		stop_move = false;
		float len = iTween.PathLength(path);
		for(int i=0; i < parts.Length; i++ ) {
			parts[i].Reset();
			parts[i].Prepare(path,len,(parts[i].transform.position - path[0]).magnitude);
		}
	}

	public bool MovePerFrame(Vector3[] path,float deltaTime) {
		if ( stop_move ) return false;
		for(int i=0; i < parts.Length; i++ ) {
			if ( parts[i].MovePerFrame(path,speed,deltaTime) ) {
				Debug.Log("Part "+ i + " reached the end of path, stop moving ");
				stop_move = true;
				return false;
			}
		}
		return true;
	}
}

