using UnityEngine;
using System.Collections;

public class MotorMovement : MonoBehaviour 
{
	public Rigidbody objectBody;
	private float sineSeed = 0.0f;
	private float rotationSeed = 0.0f;
	private float vehicleRotationFactor = 0.0f;
	private float vehicleAccelerationFactor = 0.0f;

	// Use this for initialization
	void Start () 
	{
	}
	
	// Update is called once per frame
	void Update () 
	{
		objectBody.transform.Translate (Vector3.up * Time.deltaTime * Mathf.Sin (sineSeed) * 0.05f);
		sineSeed = ((sineSeed + 15.0f * Time.deltaTime) % 360.0f) * 0.01f;

		rotationSeed = rotationSeed + Input.GetAxis ("Horizontal") * Time.deltaTime;

		vehicleRotationFactor = 
			Input.GetAxis ("Horizontal") * 0.5f * (
				objectBody.velocity [0] * objectBody.velocity [0] +
			objectBody.velocity [2] * objectBody.velocity [2]
				);

		vehicleRotationFactor = (vehicleRotationFactor >  2.0f) ?  1.5f : vehicleRotationFactor;
		vehicleRotationFactor = (vehicleRotationFactor < -2.0f) ? -2.0f : vehicleRotationFactor;

		vehicleAccelerationFactor = 
			((Input.GetAxis ("Vertical") < -0.9f)?
				-0.9f:
				Input.GetAxis("Vertical") 
			) * 28.0f;

		objectBody.transform.Rotate (
			new Vector3 (0, 1, 0), 
			vehicleRotationFactor
		);

		objectBody.AddRelativeForce (Vector3.right * vehicleAccelerationFactor);
	}	
}
