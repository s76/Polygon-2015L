using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour, IPointerExitHandler {

	public GameObject openInventoryButton, closeInventoryButton;
	public int inventoryRows, inventoryColumns;
	public GameObject buttonPrefab;
	public GameObject inventoryGrid, quickslotPanel;

	private bool isInventoryOpened;
	private GameObject[] buttons;
	
	void Start () {
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
}
