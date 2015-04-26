using UnityEngine;
using System.Collections;

public class Drive_Wheel_Self_Accelerate : MonoBehaviour {

	public bool Drive_Flag = true ;
	public float Max_Torque = 1000.0f ;
	public float Torque_Accelerate = 20.0f ;
	public float Accelerate_Rate = 300.0f ; 
	public float Brake_Drag = 5.0f ;
	public float Idle_Drag = 0.1f ;
	public float maxAngularVelocity = 50.0f ;

	Rigidbody This_RigidBody ;
	float Target_Torque ;
	float Current_Torque ;

	void Start () {
		This_RigidBody = this.GetComponent<Rigidbody>() ;
		This_RigidBody.maxAngularVelocity = maxAngularVelocity;
		This_RigidBody.angularDrag = Idle_Drag ;
		// Layer settings.
		this.gameObject.layer = 8 ;
		Physics.IgnoreLayerCollision ( 0 , 8 , true ) ;
		Physics.IgnoreLayerCollision ( 8 , 8 , false ) ;
	}

	void Update () {
		// Accelerate
		if ( Drive_Flag ) {
			Target_Torque = Mathf.Lerp ( 0.0f , Max_Torque , Torque_Accelerate ) ;
		}

		if ( Current_Torque < Target_Torque ) {
			Current_Torque = Mathf.MoveTowards ( Current_Torque , Target_Torque , Accelerate_Rate * Time.deltaTime ) ;
		} else if ( Current_Torque > Target_Torque ) {
			Current_Torque = Target_Torque ;
		}
	}

	void FixedUpdate () {
		GetComponent<Rigidbody>().AddRelativeTorque ( Current_Torque , 0 , 0 ) ;
	}

}
