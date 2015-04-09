using UnityEngine;
using System.Collections;

/* s76
 * próba poprawienia ItemScript.cs 
 */ 
public abstract class AbsItem : MonoBehaviour
{
	abstract public void OnBeingPicked ();
	abstract public void OnBeingThrowed ();
	abstract public void OnBeingUsed ();
	abstract public void OnBeingNotUsed ();
}

