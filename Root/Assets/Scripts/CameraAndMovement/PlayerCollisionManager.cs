using UnityEngine;
using System.Collections;

public class PlayerCollisionManager : MonoBehaviour {

    public GameObject inventoryBuilderObject;
    private InventoryBuilder inventoryBuilder;

    void Start() {
        inventoryBuilder = inventoryBuilderObject.GetComponent<InventoryBuilder>();
    }

    void OnTriggerStay(Collider other) {
        if (other.tag == "Item") {
            if (Input.GetButton("Use")) {
				GameObject itemObject = other.transform.gameObject;
                ItemScript itemScript= other.GetComponentInParent<ItemScript>();
				bool wasItemPickedUp = inventoryBuilder.AddItemToInventory(itemObject);
				if(wasItemPickedUp){
                    other.transform.parent = transform; //przedmiot ma isc za graczem?
					InfoPanelManager.AddNewMessage(Strings.pickUpItem + itemScript.GetItem().GetName());
				}
            }
        }

    }

}
