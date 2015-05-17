using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class Axe : AbsItem
{
	public float rotateSpeed = 50;
	bool swing;
	Animator animator;

    public  Axe() : base("Axe", false) { } 

	void Start () {
		animator = GetComponent<Animator>();
	} 

	public override void OnBeingPicked () 
	{
		Debug.Log ("Axe picked");
		PutOutOfSight();
        //GetComponent<SphereCollider>().enabled = false;
       // GetComponent<MeshRenderer>().enabled = false;
	}

	public override void OnBeingThrown ()
	{
		Debug.Log("Axe throwed");
		ThrowAway();
        // GetComponent<SphereCollider>().enabled = true;
        // GetComponent<MeshRenderer>().enabled = true;
	}

	public override void OnBeingAssociatedToHand(string hand)
	{
		PutInHand(hand);
	}

	public override void OnBeingRemovedFromHand(){
		PutOutOfSight();
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
		if (!swing) {
			return;
		}
		swing = false;
		animator.SetBool("swing",swing);
		transform.localRotation = Quaternion.identity;
	}
	
}

