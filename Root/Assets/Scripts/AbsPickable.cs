using UnityEngine;
using System.Collections;

public abstract class AbsPickable : MonoBehaviour
{
	protected static Vector3 INVENTORY_LOCATION = new Vector3(10.0f, 0.0f, 10.0f);

	private string pickableName;
	
	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public AbsPickable(string name){
		this.pickableName = name;
	}
	
	public string GetName() {
		return pickableName;
	}

	abstract public void OnBeingPicked ();
	
	protected void PutOutOfSight(){
		transform.parent = null;
		transform.localPosition = INVENTORY_LOCATION;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		gameObject.SetActive(false);
		//Debug.Log (transform.localScale);
	}

}

