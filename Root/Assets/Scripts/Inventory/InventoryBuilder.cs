using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class InventoryBuilder : AbsInventoryBuilder, IPointerExitHandler  {

	public const string LEFT = "Left";
	public const string RIGHT = "Right";
	public const string BOTH = "Both";

	public GameObject openInventoryButton, closeInventoryButton;
	public int inventoryRows = 4;
	public int inventoryColumns = 3;
	public GameObject buttonPrefab;
	public GameObject inventoryGrid, quickslotPanel;
	
	private bool isInventoryOpened;
	private GameObject[] buttons;
    private List<GameObject> pickedUpObjects;

	void Start () {
		leftHand = GameObject.Find("LeftHandIcon");
		rightHand = GameObject.Find("RightHandIcon");
	
		if (leftHand == null || rightHand == null) {
			throw new UnassignedReferenceException("left or right hand icon object is missing");
		}
		isInventoryOpened = false;
		openInventoryButton.gameObject.SetActive (true);
		closeInventoryButton.gameObject.SetActive (false);
		
		buttons = new GameObject[inventoryRows * inventoryColumns];
		
		for (int i=0; i<inventoryRows * inventoryColumns; i++) {
			GameObject b = Instantiate (buttonPrefab);
			buttons[i] = b;
			b.GetComponentInChildren<Text> ().text =  i.ToString();
		}

        pickedUpObjects = new List<GameObject>();

		onCloseInventoryButtonClicked ();

	}
	
	public void onShowInventoryButtonClicked(){
		isInventoryOpened = true;
		/* odkomentowanie tych 2 linii spowoduje ze bedzie sie pojawial przycisk zamkniecia ekwpinuku
		closeInventoryButton.gameObject.SetActive (true);
		openInventoryButton.gameObject.SetActive (false);
         */
		inventoryGrid.gameObject.SetActive (true);
		quickslotPanel.gameObject.SetActive (false);
		
		for (int i=0; i<buttons.Length; i++) {
			GameObject b = buttons[i];
			b.transform.SetParent(inventoryGrid.transform, false);
		}
		
	}
	
	public void onCloseInventoryButtonClicked(){
		isInventoryOpened = false;
		closeInventoryButton.gameObject.SetActive (false);
		openInventoryButton.gameObject.SetActive (true);
		inventoryGrid.gameObject.SetActive (false);
		quickslotPanel.gameObject.SetActive (true);
		
		for (int i=0; i<inventoryRows; i++) {
			GameObject b = buttons[i];
			b.transform.SetParent(quickslotPanel.transform, false);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		isInventoryOpened = false;
		closeInventoryButton.gameObject.SetActive(false);
		openInventoryButton.gameObject.SetActive(true);
		inventoryGrid.gameObject.SetActive(false);
		quickslotPanel.gameObject.SetActive(true);
		
		for (int i = 0; i < inventoryRows; i++)
		{
			GameObject b = buttons[i];
			b.transform.SetParent(quickslotPanel.transform, false);
		}
	}

	public void AssociateItemToHand(AbsItem item, string hand) {
		InventoryButtonScript leftHandScript = leftHand.GetComponent<InventoryButtonScript> ();
		InventoryButtonScript rightHandScript = rightHand.GetComponent<InventoryButtonScript> ();
		switch (hand) {
		case LEFT:
			if (leftHandScript.GetAssociatedItem () == item) {
				RemoveAssociatedItemFromHand (leftHand);
				item.OnBeingRemovedFromHand ();
			} else {
				if (rightHandScript.GetAssociatedItem () == item) {
					RemoveAssociatedItemFromHand (rightHand);
				}
				if (leftHandScript.GetAssociatedItem () != null) {
					leftHandScript.GetAssociatedItem ().OnBeingRemovedFromHand ();
					if (leftHandScript.GetAssociatedItem ().IsDoubleHanded ()) {
						RemoveAssociatedItemFromHand (rightHand);
					}
					RemoveAssociatedItemFromHand (leftHand);
				}
				item.OnBeingAssociatedToHand (LEFT);
				leftHandScript.SetAssociatedItem (item);
			}
			break;
		case RIGHT:
			if (rightHandScript.GetAssociatedItem () == item) {
				RemoveAssociatedItemFromHand (rightHand);
				item.OnBeingRemovedFromHand ();
			} else {
				if (leftHandScript.GetAssociatedItem () == item) {
					RemoveAssociatedItemFromHand (leftHand);
				}
				if (rightHandScript.GetAssociatedItem () != null) {
					rightHandScript.GetAssociatedItem ().OnBeingRemovedFromHand ();
					if (rightHandScript.GetAssociatedItem ().IsDoubleHanded ()) {
						RemoveAssociatedItemFromHand (leftHand);
					}
					RemoveAssociatedItemFromHand (rightHand);
				}
				item.OnBeingAssociatedToHand (RIGHT);
				rightHandScript.SetAssociatedItem (item);
			}
			break;
		case BOTH:
			if (leftHandScript.GetAssociatedItem () == item || rightHandScript.GetAssociatedItem () == item) {
				RemoveAssociatedItemFromHand (leftHand);
				RemoveAssociatedItemFromHand (rightHand);
			} else {
				if (leftHandScript.GetAssociatedItem () != null) {
					leftHandScript.GetAssociatedItem ().OnBeingRemovedFromHand ();
					RemoveAssociatedItemFromHand (leftHand);
				}
				if (rightHandScript.GetAssociatedItem () != null) {
					rightHandScript.GetAssociatedItem ().OnBeingRemovedFromHand ();
					RemoveAssociatedItemFromHand (rightHand);
				}
				item.OnBeingAssociatedToHand (BOTH);
				leftHandScript.SetAssociatedItem (item);
				rightHandScript.SetAssociatedItem (item);
			}
			break;
		}
	}
	
	private void RemoveAssociatedItemFromHand(GameObject hand) {
		InventoryButtonScript script = hand.GetComponent<InventoryButtonScript>();
		script.SetAssociatedItem(null);
		hand.GetComponentInChildren<Text>().text = "hand";
	}
	
	public bool AddItemToInventory(GameObject itemObject) {
		if (pickedUpObjects.Contains (itemObject)) {
			return false;
		}
		GameObject freeButton = FindFreeInventorySlot();
		if (freeButton == null) {
			return false;
		}
        AbsItem item = itemObject.GetComponent<AbsItem>();
		freeButton.GetComponent<InventoryButtonScript> ().SetAssociatedItem (item);
        pickedUpObjects.Add(itemObject);
		return true;
	}
	
	private GameObject FindFreeInventorySlot() {
		foreach (GameObject button in buttons) {
			if(button.GetComponent<InventoryButtonScript>().IsFreeSlot()){
				return button;
			}
		}
		return null;
	}

    public void RemoveItemFromInventory(AbsItem item){
        InfoPanelManager.AddNewMessage(Strings.dropItem + item.GetName());

        if (leftHand.GetComponent<InventoryButtonScript>().GetAssociatedItem() == item) {
            RemoveAssociatedItemFromHand(leftHand);
        }
        if (rightHand.GetComponent<InventoryButtonScript>().GetAssociatedItem() == item)
        {
            RemoveAssociatedItemFromHand(rightHand);
        }

        foreach (GameObject itemObject in pickedUpObjects){
            if (itemObject.GetComponent<AbsItem>() == item){
                itemObject.GetComponent<AbsItem>().OnBeingThrown();
                itemObject.transform.SetParent(null, true);
                pickedUpObjects.Remove(itemObject);
                return;
            }
        }
    }

    public void UseLeftHandItem() {
        if (leftHand.GetComponent<InventoryButtonScript>().GetAssociatedItem() != null) {
            leftHand.GetComponent<InventoryButtonScript>().GetAssociatedItem().OnBeingUsed();
        }
    }

    public void UseRightHandItem() {
        if (rightHand.GetComponent<InventoryButtonScript>().GetAssociatedItem() != null) {
            rightHand.GetComponent<InventoryButtonScript>().GetAssociatedItem().OnBeingUsed();
        }
    }
}