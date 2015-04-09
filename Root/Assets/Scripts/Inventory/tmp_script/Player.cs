using UnityEngine;
using System.Collections;


public class Player : MonoBehaviour
{
	public float pickUpRange;
	public Transform leftHand;
	public Transform rightHand;
	Collider intouchItem;
	AbsItem pickedUpItem;

	void Update () {
		if ( Input.GetKeyUp(KeyCode.F) & intouchItem != null ){
			pickedUpItem = intouchItem.GetComponent<AbsItem>();
			pickedUpItem.OnBeingPicked();
		}

		if ( pickedUpItem != null ) {
			if ( Input.GetMouseButton(0)) {
				Debug.Log("using item");
				pickedUpItem.OnBeingUsed();
			}
			else {
				Debug.Log("not using item");
				pickedUpItem.OnBeingNotUsed();
			}
		} 
		
	}

	void OnTriggerEnter ( Collider other ) {
		if ( other.CompareTag("Item") ) {
			Debug.Log("enter");
			intouchItem = other;
		}
	}
	
	void OnTriggerExit ( Collider other ) {
		if ( other.CompareTag("Item") ) {
			Debug.Log("exit");
			intouchItem = null;
		}
	}

	#region singleton
	static private Player _instance;
	static public Player Instance {
		get {
			if ( _instance == null ) {
				_instance = GameObject.FindObjectOfType(typeof(Player)) as Player;
			}
			return _instance;
		}
	}
	#endregion
}

