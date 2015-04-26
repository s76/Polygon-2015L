using UnityEngine;
using System.Collections;

/* s76
 * próba poprawienia ItemScript.cs 
 */ 
public abstract class AbsItem : MonoBehaviour
{
	protected static Vector3 INVENTORY_LOCATION = new Vector3(10.0f, 0.0f, 10.0f);

    protected bool isDoubleHanded;
    protected string itemName;

	abstract public void OnBeingPicked ();
	abstract public void OnBeingThrown ();
	abstract public void OnBeingAssociatedToHand (string hand);
	abstract public void OnBeingRemovedFromHand ();
	abstract public void OnBeingUsed ();
	abstract public void OnBeingNotUsed ();

    protected AbsItem( string name, bool isDoubleHanded) {
		this.itemName = name;
		this.isDoubleHanded = isDoubleHanded;
	}

    public bool IsDoubleHanded() {
        return isDoubleHanded;
    }

    public string GetName() {
        return itemName;
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

	protected void PutOutOfSight(){
		transform.parent = null;
		transform.localPosition = INVENTORY_LOCATION;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
        gameObject.SetActive(false);
		//Debug.Log (transform.localScale);
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

