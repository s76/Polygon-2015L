using UnityEngine;
using System.Collections;

public class RaycastingTest : MonoBehaviour
{
	
	static Plane XZPlane = new Plane(Vector3.up, Vector3.zero);

	// Use this for initialization
	void Start ()
	{
		if (Input.GetMouseButton (1)) {
			Vector3 target = GetMousePositionOnXZPlane ();
			//rotation.SetLookRotation (target);
		} 
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public static Vector3 GetMousePositionOnXZPlane() {
		float distance;
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		if(XZPlane.Raycast (ray, out distance)) {
			Vector3 hitPoint = ray.GetPoint(distance);
			//Just double check to ensure the y position is exactly zero
			hitPoint.y = 0;
			Debug.Log(hitPoint);
			return hitPoint;
		}
		return Vector3.zero;
	}
}

