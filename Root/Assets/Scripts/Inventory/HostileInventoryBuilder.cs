using UnityEngine;
using System.Collections;

public class HostileInventoryBuilder : AbsInventoryBuilder
{

	// Use this for initialization
	void Start ()
	{
		leftHand = GameObject.Find("LeftHandIcon");
		rightHand = GameObject.Find("RightHandIcon");
		
		if (leftHand == null || rightHand == null) {
			throw new UnassignedReferenceException("left or right hand icon object is missing");
		}
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}
}

