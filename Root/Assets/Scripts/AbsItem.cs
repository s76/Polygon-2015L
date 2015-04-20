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
	abstract public void OnBeingThrowed ();
	abstract public void OnBeingAssociatedToHand (string hand);
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
}

