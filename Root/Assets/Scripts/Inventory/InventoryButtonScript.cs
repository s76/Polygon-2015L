using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class InventoryButtonScript : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    private InventoryBuilder inventoryBuilder;
	private AbsItem associatedItem;
    private bool isPointerOverButton;

	void Start () {
        GameObject inventoryPanelObject = GameObject.Find("Inventory Panel");
        if (inventoryPanelObject == null) {
            throw new UnassignedReferenceException("inventory builder object missing");
        }

        inventoryBuilder = inventoryPanelObject.GetComponent<InventoryBuilder>();
	}


	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left && associatedItem != null) {
            inventoryBuilder.AssociateItemToHand(associatedItem, InventoryBuilder.LEFT);
		} else if (eventData.button == PointerEventData.InputButton.Middle) {
            if (isPointerOverButton && associatedItem != null){
                inventoryBuilder.RemoveItemFromInventory(this.associatedItem);
                SetAssociatedItem(null);
            }
		} else if (eventData.button == PointerEventData.InputButton.Right && associatedItem != null) {
			inventoryBuilder.AssociateItemToHand(associatedItem, InventoryBuilder.RIGHT);
		}
	}

	public bool IsFreeSlot(){
		return associatedItem == null;
	}

    public AbsItem GetAssociatedItem() {
        return associatedItem;
    }

    //przypisuje obrazek, wskaznik itp.
    public void SetAssociatedItem(AbsItem item) {
        associatedItem = item;
        if (item != null) {
            GetComponentInChildren<Text>().text = item.GetName();
        }
        else{
            GetComponentInChildren<Text>().text = "free";
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isPointerOverButton = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isPointerOverButton = true;
    }
}
