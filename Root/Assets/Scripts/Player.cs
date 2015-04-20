using UnityEngine;
using System.Collections;


public class Player : MonoBehaviour
{
	public float pickUpRange;
	public Transform leftHand;
	public Transform rightHand;
    public GameObject inventoryBuilderObject;
    private InventoryBuilder inventoryBuilder;

    void Start() {
        inventoryBuilder = inventoryBuilderObject.GetComponent<InventoryBuilder>();
    }

    void Update() {

        //rozwiazanie tymaczasowe - tu tak byc nie moze, bo nawet jezeli klikniecie bylo na przycisk, to i tak zostaje uzyty przedmiot
        if (Input.GetMouseButton(0)) {
            Debug.Log("using item");
            inventoryBuilder.UseLeftHandItem();
        }
        else if (Input.GetMouseButton(1)){
            Debug.Log("not using item");
            inventoryBuilder.UseRightHandItem();
        }
    }

    void OnTriggerStay(Collider other) {
		Debug.Log ("Colliding with item");
        if (other.tag == "Item") {
            if (Input.GetButton("Use")) {
                GameObject itemObject = other.transform.gameObject;
                AbsItem absItem = other.GetComponentInParent<AbsItem>();
                bool wasItemPickedUp = inventoryBuilder.AddItemToInventory(itemObject);
                if (wasItemPickedUp) {
                    //other.transform.parent = transform; //przedmiot ma isc za graczem?
                    absItem.OnBeingPicked();
                    InfoPanelManager.AddNewMessage(Strings.pickUpItem + absItem.GetName());
                }
            }
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

