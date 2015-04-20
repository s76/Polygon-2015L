using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class Gun : AbsGunItem
{
	public float rotateSpeed = 20;
	bool swing;
	Animator animator;

    public  Gun() : base("Gun", false) { } 

	void Start () {
		animator = GetComponent<Animator>();
	}

	public override void OnBeingPicked ()
	{
		Debug.Log("gun picked");
		transform.localPosition = INVENTORY_LOCATION;
		transform.localRotation = Quaternion.identity;

        //GetComponent<SphereCollider>().enabled = false;
       // GetComponent<MeshRenderer>().enabled = false;
	}

	public override void OnBeingThrowed ()
	{
		Debug.Log("gun throwed");
        // GetComponent<SphereCollider>().enabled = true;
        // GetComponent<MeshRenderer>().enabled = true;
	}

	public override void OnBeingAssociatedToHand(string hand)
	{
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		if(hand.Equals("Left")){
			Debug.Log("gun to left hand");
			transform.parent = Player.Instance.leftHand;
		} else if (hand.Equals("Right")){
			Debug.Log("gun to right hand");
			transform.parent = Player.Instance.rightHand;
		} else {
			throw new ArgumentException("Allowed arguments: 'Left', 'Right'");
		}
	}

	public override void OnBeingUsed ()
	{
		if (!swing ) {
			swing = true;
			animator.SetBool("swing",swing);
		}
	}

	public override void OnBeingNotUsed ()
	{
		if (!swing ) return;
		swing = false;
		animator.SetBool("swing",swing);
		transform.localRotation = Quaternion.identity;
	}
	
}

