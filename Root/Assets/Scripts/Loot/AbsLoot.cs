using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public abstract class AbsLoot : AbsPickable
{
	public enum Type { LIGHT, HEAVY };

	protected const int forceValue = 500;

	public AbsLoot(string name): base(name) {

	}

	// Use this for initialization
	void Start ()
	{

	}
	
	// Update is called once per frame
	void Update ()
	{

	}
	
	public abstract Type GetLootType();
	public abstract int GetValue();

	public void Push (Vector3 force)
	{
		Rigidbody rigidbody = GetComponent<Rigidbody> ();
		if (GetLootType () == Type.LIGHT) {
			Debug.Log("Push with force: "+force);
			rigidbody.AddForce (force * forceValue);
		}
	}
	
}

