using UnityEngine;
using System.Collections;

public class PlayerCollisionManager : MonoBehaviour {

    public GameObject inventoryBuilderObject;
    private InventoryBuilder inventoryBuilder;

    void Start() {
        inventoryBuilder = inventoryBuilderObject.GetComponent<InventoryBuilder>();
    }

    void OnTriggerStay(Collider other) {
        if (other.tag == "Item") {
            if (Input.GetButton("Use")) {
                Debug.Log("podnies");
            }
        }

    }

}
