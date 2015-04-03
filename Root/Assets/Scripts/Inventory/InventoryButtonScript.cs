using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
public class InventoryButtonScript : MonoBehaviour, IPointerClickHandler
{

    private InventoryBuilder inventoryBuilder;
	private Item associatedItem;

	void Start () {
        GameObject inventoryPanelObject = GameObject.Find("Inventory Panel");
        if (inventoryPanelObject == null) {
            throw new UnassignedReferenceException("inventory builder object missing");
        }

        inventoryBuilder = inventoryPanelObject.GetComponent<InventoryBuilder>();
	}


	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left) {
            inventoryBuilder.AssociateItemToHand(associatedItem, "Left");
		} else if (eventData.button == PointerEventData.InputButton.Middle) {
			//Debug.Log ("Middle click");
		} else if (eventData.button == PointerEventData.InputButton.Right) {
            inventoryBuilder.AssociateItemToHand(associatedItem, "Right");
			
		}
	}


    public Item GetAssociatedItem() {
        return associatedItem;
    }

    //przypisuje obrazek, wskaznik itp.
    public void SetAssociatedItem(Item item) {
        associatedItem = item;
        if (item != null) { 
            GetComponentInChildren<Text>().text = item.GetName();
        }
    }


}
