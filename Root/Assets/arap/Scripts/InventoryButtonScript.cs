using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryButtonScript : MonoBehaviour , IPointerClickHandler {
	
	public GameObject associatedItem;

	private GameObject leftHand, rightHand;

	void Start () {
		leftHand = GameObject.Find("LeftHand");
		rightHand = GameObject.Find("RightHand");

		if(leftHand == null || rightHand == null){
			throw new UnassignedReferenceException("left or right hand object is missing");
		}
	}


	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Left) {
			//leftHand.GetComponent<InventoryButtonScript>().associatedItem = associatedItem;
			leftHand.GetComponentInChildren<Text>().text = GetComponentInChildren<Text>().text;
		} else if (eventData.button == PointerEventData.InputButton.Middle) {
			//Debug.Log ("Middle click");
		} else if (eventData.button == PointerEventData.InputButton.Right) {
			//rightHand.GetComponent<InventoryButtonScript>().associatedItem = associatedItem;
			rightHand.GetComponentInChildren<Text>().text = GetComponentInChildren<Text>().text;
		}
	}
}
