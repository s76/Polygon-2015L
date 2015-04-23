using UnityEngine;
using System.Collections;

public class Axe : AbsItem
{
	public float rotateSpeed = 20;
	bool swing;
	Animator animator;

    public  Axe() : base("Axe", false) { } 

	void Start () {
		animator = GetComponent<Animator>();
	} 

	public override void OnBeingPicked () 
	{
		Debug.Log("axe picked");
		transform.parent = Player.Instance.leftHand;
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;

        //GetComponent<SphereCollider>().enabled = false;
       // GetComponent<MeshRenderer>().enabled = false;
	}

	public override void OnBeingThrowed ()
	{
		Debug.Log("axe throwed");
        // GetComponent<SphereCollider>().enabled = true;
        // GetComponent<MeshRenderer>().enabled = true;
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

