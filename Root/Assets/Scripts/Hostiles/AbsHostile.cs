using UnityEngine;
using System.Collections;

public abstract class AbsHostile : AbsCharacter
{
	protected string hostileName;
	
	public GameObject leftHandObject, rightHandObject;

	protected AbsHostile(string name, int hitpoints): base(hitpoints){
		this.hostileName = name;
	}

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

}

