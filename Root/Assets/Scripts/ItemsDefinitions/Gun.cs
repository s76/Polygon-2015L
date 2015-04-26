using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class Gun : AbsGunItem
{
	private GameObject leftHand;
	private GameObject rightHand;

    public  Gun() : base("Gun", false, 20, 0.15f, 250.0f) { } 
	
	void Start () {
		leftHand = GameObject.Find("LeftHandIcon");
		rightHand = GameObject.Find("RightHandIcon");
	}

	public override void OnBeingPicked ()
	{
		PutOutOfSight ();
        //GetComponent<SphereCollider>().enabled = false;
       // GetComponent<MeshRenderer>().enabled = false;
	}

	public override void OnBeingThrown ()
	{
		armed = false;
		ThrowAway ();
        // GetComponent<SphereCollider>().enabled = true;
        // GetComponent<MeshRenderer>().enabled = true;
	}

	public override void OnBeingAssociatedToHand(string hand)
	{
		armed = true;
		PutInHand(hand);
		//Vector3 rotation = transform.eulerAngles;
		//rotation.x = 90.0f;
		//transform.eulerAngles = rotation;
	}
	
	public override void OnBeingRemovedFromHand(){
		armed = false;
		PutOutOfSight();
	}

	public override void OnBeingUsed ()
	{
	}

	public override void OnBeingNotUsed ()
	{
	}
	
}

