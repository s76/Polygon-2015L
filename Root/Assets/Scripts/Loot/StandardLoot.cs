using UnityEngine;
using System.Collections;

public class StandardLoot : AbsLoot
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public StandardLoot(): base("Standard loot"){

	}

	public override Type GetLootType ()
	{
		return Type.LIGHT;
	}

	public override void OnBeingPicked ()
	{
		PutOutOfSight ();
	}
}

