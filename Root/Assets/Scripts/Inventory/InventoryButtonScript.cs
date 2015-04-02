using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryButtonScript : MonoBehaviour, IPointerClickHandler
{
	
	private Item associatedItem;

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
            AssociateItemToHand(associatedItem, leftHand);
		} else if (eventData.button == PointerEventData.InputButton.Middle) {
			//Debug.Log ("Middle click");
		} else if (eventData.button == PointerEventData.InputButton.Right) {
            AssociateItemToHand(associatedItem, rightHand);
			
		}
	}

    //przypisuje obrazek, wskaznik itp.
    public void SetAssociatedItem(Item item) {
        if (item == null) return;
        associatedItem = item;
        GetComponentInChildren<Text>().text = item.GetName();
    }

    private void RemoveAssociatedItem(GameObject hand) {
        hand.GetComponent<InventoryButtonScript>().associatedItem = null;
        hand.GetComponentInChildren<Text>().text = "hand";
    }

    public Item GetAssociatedItem() {
        return associatedItem;
    }

    private void AssociateItemToHand(Item item, GameObject handToAssociate) {
        InventoryButtonScript leftHandScript = leftHand.GetComponent<InventoryButtonScript>();
        InventoryButtonScript rightHandScript = rightHand.GetComponent<InventoryButtonScript>();

        if (item.IsDoubleHanded()) {
            //jezeli obiekt juz jest w obu rekach - usun go 
            if (leftHandScript.GetAssociatedItem() == item && rightHandScript.GetAssociatedItem() == item) {
                RemoveAssociatedItem(leftHand);
                RemoveAssociatedItem(rightHand);
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
                RemoveAssociatedItem(handToAssociate);
            }
            //jezeli obiekt juz znajduje sie w innej rece - nalezy przypisac go do nowej reki
            else if (handToAssociate == leftHand && rightHandScript.GetAssociatedItem() == item) {
                RemoveAssociatedItem(rightHand);
                AssociateItemToHand(item, leftHand);
            }
            else if (handToAssociate == rightHand && leftHandScript.GetAssociatedItem() == item) {
                RemoveAssociatedItem(leftHand);
                AssociateItemToHand(item, rightHand);
            }
            //przypisywanego obiektu nie ma w zadnej z rak - nalezy go dodac, ewentualnie usuwajac aktualnie sie znajdujacy
            else {
                //w rekach byl wczesniej obiekt dwureczny - nalezy go usunac z obu
                if ((leftHandScript.GetAssociatedItem() != null && leftHandScript.GetAssociatedItem().IsDoubleHanded()) ||
                    (rightHandScript.GetAssociatedItem() != null && rightHandScript.GetAssociatedItem().IsDoubleHanded())) {
                        RemoveAssociatedItem(leftHand);
                        RemoveAssociatedItem(rightHand);
                }

                handToAssociate.GetComponent<InventoryButtonScript>().SetAssociatedItem(item);
            }
        }
        
    }


}
