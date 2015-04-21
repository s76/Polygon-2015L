using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class InventoryBuilder : MonoBehaviour, IPointerExitHandler  {
	
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

	public void AssociateItemToHand(AbsItem item, string whichHand) {
		GameObject handToAssociate;
		if (whichHand.Equals("Left")) {
			handToAssociate = leftHand;
		}
		else if (whichHand.Equals("Right")) {
			handToAssociate = rightHand;
		}
		else {
			throw new ArgumentException("Allowed arguments: 'Left', 'Right'");
		}


		InventoryButtonScript leftHandScript = leftHand.GetComponent<InventoryButtonScript>();
		InventoryButtonScript rightHandScript = rightHand.GetComponent<InventoryButtonScript>();
		
		if (item.IsDoubleHanded()) {
			//jezeli obiekt juz jest w obu rekach - usun go 
			if (leftHandScript.GetAssociatedItem() == item && rightHandScript.GetAssociatedItem() == item) {
				RemoveAssociatedItemFromHand(leftHand);
				RemoveAssociatedItemFromHand(rightHand);
			}
			//jezeli obiektu nie ma w zadnej z rak - dodaj go do obu
			else {
				leftHandScript.SetAssociatedItem(item);
				rightHandScript.SetAssociatedItem(item);
			}
			
		}
		else {
			//przypsiwywany obiekt juz znajduje sie w przypisywanej rece - nalezy go usunac z reki
			if (handToAssociate.GetComponent<InventoryButtonScript>().GetAssociatedItem() == item) {
				RemoveAssociatedItemFromHand(handToAssociate);
			}
			//jezeli obiekt juz znajduje sie w innej rece - nalezy przypisac go do nowej reki
			else if (handToAssociate == leftHand && rightHandScript.GetAssociatedItem() == item) {
				RemoveAssociatedItemFromHand(rightHand);
				leftHandScript.SetAssociatedItem(item);
			}
			else if (handToAssociate == rightHand && leftHandScript.GetAssociatedItem() == item) {
				RemoveAssociatedItemFromHand(leftHand);
				rightHandScript.SetAssociatedItem(item);
			}
			//przypisywanego obiektu nie ma w zadnej z rak - nalezy go dodac, ewentualnie usuwajac aktualnie sie znajdujacy
			else {
				//w rekach byl wczesniej obiekt dwureczny - nalezy go usunac z obu
				if ((leftHandScript.GetAssociatedItem() != null && leftHandScript.GetAssociatedItem().IsDoubleHanded()) ||
				    (rightHandScript.GetAssociatedItem() != null && rightHandScript.GetAssociatedItem().IsDoubleHanded())) {
					RemoveAssociatedItemFromHand(leftHand);
					RemoveAssociatedItemFromHand(rightHand);
				}
				
				handToAssociate.GetComponent<InventoryButtonScript>().SetAssociatedItem(item);
			}
		}	
		
	}
	
	private void RemoveAssociatedItemFromHand(GameObject hand) {
		hand.GetComponent<InventoryButtonScript>().SetAssociatedItem(null);
		hand.GetComponentInChildren<Text>().text = "hand";
	}
	
	public bool AddItemToInventory(GameObject itemObject) {
		GameObject freeButton = FindFreeInventorySlot();
		if (freeButton == null)
			return false;

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
                itemObject.GetComponent<AbsItem>().OnBeingThrowed();
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