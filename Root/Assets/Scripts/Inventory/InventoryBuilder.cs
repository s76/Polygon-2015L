using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using System;

public class InventoryBuilder : MonoBehaviour, IPointerExitHandler {

	public GameObject openInventoryButton, closeInventoryButton;
	public int inventoryRows, inventoryColumns;
	public GameObject buttonPrefab;
	public GameObject inventoryGrid, quickslotPanel;

	private bool isInventoryOpened;
    private GameObject leftHand, rightHand;
	private GameObject[] buttons;
	
	void Start () {
        leftHand = GameObject.Find("LeftHand");
        rightHand = GameObject.Find("RightHand");

        if (leftHand == null || rightHand == null) {
            throw new UnassignedReferenceException("left or right hand object is missing");
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

		onCloseInventoryButtonClicked ();


        buttons[0].GetComponent<InventoryButtonScript>().SetAssociatedItem(new Gun1("jednoreczny 1 ", false));
        buttons[1].GetComponent<InventoryButtonScript>().SetAssociatedItem(new Gun2("jednoreczny 2", false));
        buttons[2].GetComponent<InventoryButtonScript>().SetAssociatedItem(new Gun2("dwureczny 1", true));
        buttons[3].GetComponent<InventoryButtonScript>().SetAssociatedItem(new Gun2("dwureczny 2", true));
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




    public void AssociateItemToHand(Item item, string whichHand) {
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

    public void PickItem() {

    }

    private void FindFreeInventorySlot() {

    }
}
