using UnityEngine;
using System.Collections;

public class MovableOnPath : MonoBehaviour
{
	int current;
	float frag_len;
	 
	void Start () {
		Reset(); 
	}

	public void Reset() {
		current = 1;
		frag_len = 0;
	}

	/* for proper behavior : 
	 * 		object must already locate on path, 
	 * 		the path fragment , on which the object is locating , must be a straight section 
	 */
	public void Prepare (Vector3[] path,float path_len, float start_pos) {
		float k = 0, _s = 0;
		while (  true ) {
			k = (path[current] - path[current-1]).magnitude;
			_s += k;
			if ( _s > start_pos ) break;
			if ( current + 1 >= path.Length ) break;
			current ++;
		}
		frag_len = k - _s + start_pos;
	}

	public bool MovePerFrame(Vector3[] path,float speed, float deltaTime) {
		if ( current + 1 >= path.Length ) return true;
		float s = speed*deltaTime + frag_len;
		float k = 0, _s = 0;
		while (  true ) {
			k = (path[current] - path[current-1]).magnitude;
			_s += k;
			if ( _s > s ) break;
			if ( current + 1 >= path.Length ) break;
			current ++;
		}
		frag_len = k - _s + s;
		transform.position = Vector3.Lerp(path[current-1],path[current],frag_len/k);
		transform.LookAt(path[current],Vector3.left);
		return false;
	}
}

