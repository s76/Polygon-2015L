using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class InventoryManager : MonoBehaviour {

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
			b.GetComponentInChildren<Text> ().text = (i + 1) + "";
		}

		onCloseInventoryButtonClicked ();
	}
	
	public void onShowInventoryButtonClicked(){
		isInventoryOpened = true;
		closeInventoryButton.gameObject.SetActive (true);
		openInventoryButton.gameObject.SetActive (false);
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
	
}
