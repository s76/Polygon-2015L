using UnityEngine;
using System.Collections;

/* s76
 * próba poprawienia ItemScript.cs 
 */ 
public abstract class AbsItem : AbsPickable
{
    protected bool isDoubleHanded;
	
	abstract public void OnBeingThrown ();
	abstract public void OnBeingAssociatedToHand (string hand);
	abstract public void OnBeingRemovedFromHand ();
	abstract public void OnBeingUsed ();
	abstract public void OnBeingNotUsed ();

	protected AbsItem(string name, bool isDoubleHanded): base(name) 
	{
		this.isDoubleHanded = isDoubleHanded;
	}

    public bool IsDoubleHanded() {
        return isDoubleHanded;
    }

	protected void PutInHand(string hand){
		if (hand.Equals (InventoryBuilder.LEFT)) {
			transform.parent = Player.Instance.leftHand;
		} else if (hand.Equals (InventoryBuilder.RIGHT)) {
			transform.parent = Player.Instance.rightHand;
		} 
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
        gameObject.SetActive(true);
	}

	protected void ThrowAway(){
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		transform.parent = null;
        gameObject.SetActive(true);
        //TODO przedmiot powineien ladowac pod graczem a nie w zero
		//Debug.Log (transform.localScale);
	}
}

