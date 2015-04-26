using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class InventoryBuilder : MonoBehaviour, IPointerExitHandler  {

	public const string LEFT = "Left";
	public const string RIGHT = "Right";
	public const string BOTH = "Both";

	public GameObject openInventoryButton, closeInventoryButton;
	public int inventoryRows, inventoryColumns;
	public GameObject buttonPrefab;
	public GameObject inventoryGrid, quickslotPanel;
	
	private bool isInventoryOpened;
	private GameObject leftHand, rightHand;
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
		
		for (int i =0; i<inventoryRows * inventoryColumns; i++) {
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
		InventoryButtonScript leftHandScript = leftHand.GetComponent<InventoryButtonScript>();
		InventoryButtonScript rightHandScript = rightHand.GetComponent<InventoryButtonScript>();
		switch (hand) {
		case LEFT:
			if(leftHandScript.GetAssociatedItem() == item){
				RemoveAssociatedItemFromHand(leftHand);
				item.OnBeingRemovedFromHand();
			} else {
				if(rightHandScript.GetAssociatedItem() == item){
					RemoveAssociatedItemFromHand(rightHand);
				}
				if(leftHandScript.GetAssociatedItem() != null){
					leftHandScript.GetAssociatedItem().OnBeingRemovedFromHand();
					if(leftHandScript.GetAssociatedItem().IsDoubleHanded()){
						RemoveAssociatedItemFromHand(rightHand);
					}
					RemoveAssociatedItemFromHand(leftHand);
				}
				item.OnBeingAssociatedToHand(LEFT);
				leftHandScript.SetAssociatedItem(item);
			}
			break;
		case RIGHT:
			if(rightHandScript.GetAssociatedItem() == item){
				RemoveAssociatedItemFromHand(rightHand);
				item.OnBeingRemovedFromHand();
			} else {
				if(leftHandScript.GetAssociatedItem() == item){
					RemoveAssociatedItemFromHand(leftHand);
				}
				if(rightHandScript.GetAssociatedItem() != null){
					rightHandScript.GetAssociatedItem().OnBeingRemovedFromHand();
					if(rightHandScript.GetAssociatedItem().IsDoubleHanded()){
						RemoveAssociatedItemFromHand(leftHand);
					}
					RemoveAssociatedItemFromHand(rightHand);
				}
				item.OnBeingAssociatedToHand(RIGHT);
				rightHandScript.SetAssociatedItem(item);
			}
			break;
		case BOTH:
			if(leftHandScript.GetAssociatedItem() == item || rightHandScript.GetAssociatedItem() == item){
				RemoveAssociatedItemFromHand(leftHand);
				RemoveAssociatedItemFromHand(rightHand);
			} else {
				if(leftHandScript.GetAssociatedItem() != null){
					leftHandScript.GetAssociatedItem().OnBeingRemovedFromHand();
					RemoveAssociatedItemFromHand(leftHand);
				}
				if(rightHandScript.GetAssociatedItem() != null){
					rightHandScript.GetAssociatedItem().OnBeingRemovedFromHand();
					RemoveAssociatedItemFromHand(rightHand);
				}
				item.OnBeingAssociatedToHand(BOTH);
				leftHandScript.SetAssociatedItem(item);
				rightHandScript.SetAssociatedItem(item);
			}
			break;
		}


		/*
		GameObject handToAssociate;
		if (whichHand.Equals(LEFT)) {
			handToAssociate = leftHand;
		}
		else if (whichHand.Equals(RIGHT)) {
			handToAssociate = rightHand;
		}
		else {
			throw new ArgumentException("Allowed arguments: 'Left', 'Right'");
		}

		InventoryButtonScript leftHandScript = leftHand.GetComponent<InventoryButtonScript>();
		InventoryButtonScript rightHandScript = rightHand.GetComponent<InventoryButtonScript>();
		
		if (item.IsDoubleHanded()) {
			Debug.Log("double handed");
			//jezeli obiekt juz jest w obu rekach - usun go 
			if (leftHandScript.GetAssociatedItem() == item && rightHandScript.GetAssociatedItem() == item) {
				item.OnBeingRemovedFromHand();
				RemoveAssociatedItemFromHand(leftHand);
				RemoveAssociatedItemFromHand(rightHand);
			}
			//jezeli obiektu nie ma w zadnej z rak - dodaj go do obu
			else {
				item.OnBeingAssociatedToHand(BOTH);
				leftHandScript.SetAssociatedItem(item);
				rightHandScript.SetAssociatedItem(item);
			}
		}
		else {
			Debug.Log("one handed");
			//przypisywany obiekt juz znajduje sie w przypisywanej rece - nalezy go usunac z reki
			if (handToAssociate.GetComponent<InventoryButtonScript>().GetAssociatedItem() == item) {
				Debug.Log("removing from hand");
				RemoveAssociatedItemFromHand(handToAssociate);
				item.OnBeingRemovedFromHand();
			}
			//jezeli obiekt juz znajduje sie w innej rece - nalezy przypisac go do nowej reki
			else if (handToAssociate == leftHand && rightHandScript.GetAssociatedItem() == item) {
				Debug.Log("moving to left");
				RemoveAssociatedItemFromHand(rightHand);
				item.OnBeingAssociatedToHand(LEFT);
				leftHandScript.SetAssociatedItem(item);
			}
			else if (handToAssociate == rightHand && leftHandScript.GetAssociatedItem() == item) {
				Debug.Log("moving to right");
				RemoveAssociatedItemFromHand(leftHand);
				item.OnBeingAssociatedToHand(RIGHT);
				rightHandScript.SetAssociatedItem(item);
			}
			//przypisywanego obiektu nie ma w zadnej z rak - nalezy go dodac, ewentualnie usuwajac aktualnie sie znajdujacy
			else {
				Debug.Log("putting in hand");
				//w rekach byl wczesniej obiekt dwureczny - nalezy go usunac z obu
				if ((leftHandScript.GetAssociatedItem() != null && leftHandScript.GetAssociatedItem().IsDoubleHanded()) ||
				    (rightHandScript.GetAssociatedItem() != null && rightHandScript.GetAssociatedItem().IsDoubleHanded())) {
					Debug.Log("removing double handed");
					RemoveAssociatedItemFromHand(leftHand);
					RemoveAssociatedItemFromHand(rightHand);
				} else {
					if (handToAssociate == leftHand && leftHandScript.GetAssociatedItem() != null){
						Debug.Log("removing from left");
						RemoveAssociatedItemFromHand(leftHand);
					}
					if (handToAssociate == rightHand && rightHandScript.GetAssociatedItem() != null){
						Debug.Log("removing from right");
						RemoveAssociatedItemFromHand(rightHand);
					}
				}
				item.OnBeingAssociatedToHand(handToAssociate == leftHand ? LEFT : RIGHT);
				handToAssociate.GetComponent<InventoryButtonScript>().SetAssociatedItem(item);
			}
		}	*/
		
	}
	
	private void RemoveAssociatedItemFromHand(GameObject hand) {
		InventoryButtonScript script = hand.GetComponent<InventoryButtonScript>();
		script.SetAssociatedItem(null);
		hand.GetComponentInChildren<Text>().text = "hand";
	}
	
	public bool AddItemToInventory(GameObject itemObject) {
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