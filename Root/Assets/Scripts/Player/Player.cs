using UnityEngine;
using System.Collections;


public class Player : AbsCharacter
{
	public float pickUpRange = 5;

    public GameObject inventoryBuilderObject;
	public GameObject lootInventoryBuilderObject;

    private InventoryBuilder inventoryBuilder;
	private LootInventoryBuilder lootInventoryBuilder;

	public Player(): base(400){

	}

    void Start() {
        inventoryBuilder = inventoryBuilderObject.GetComponent<InventoryBuilder>();
		lootInventoryBuilder = lootInventoryBuilderObject.GetComponent<LootInventoryBuilder>();
    }

    void Update() {
        //rozwiazanie tymaczasowe - tu tak byc nie moze, bo nawet jezeli klikniecie bylo na przycisk, to i tak zostaje uzyty przedmiot
      /*  if (Input.GetMouseButton (0)) {
			Debug.Log ("using left item");
			//inventoryBuilder.UseLeftHandItem ();
		} 
		if (Input.GetMouseButton(1)){
            Debug.Log("using right item");
            //inventoryBuilder.UseRightHandItem();
        }*/
    }

	/** TODO
	 * - zeby loot dalo sie zebrac dopiero jak upadnie na ziemie
	 * - zeby nie dalo sie wchodzic na pociag
	 * - zeby w okienku na gorze bylo widac ikonki typow lootu i ile ich mamy
	 */

    void OnTriggerStay(Collider collider) {
		Debug.Log (collider.name);
		if (collider.tag == "Pickable") {
			Debug.Log ("pickable");
			if (Input.GetButton ("Use")) {
				GameObject itemObject = collider.transform.gameObject;
				AbsPickable pickable = collider.GetComponentInParent<AbsPickable> ();
				if(pickable is AbsItem){
					bool wasItemPickedUp = inventoryBuilder.AddItemToInventory (itemObject);
					if (wasItemPickedUp) {
						pickable.OnBeingPicked ();
						InfoPanelManager.AddNewMessage (Strings.pickUpItem + pickable.GetName ());
					}
				}
			}
			AbsLoot loot = collider.GetComponentInParent<AbsLoot> ();
			if(loot!=null){
				Debug.Log ("with loot");
				loot.OnBeingPicked();
				lootInventoryBuilder.PutLoot(loot);
				InfoPanelManager.AddNewMessage (Strings.pickUpLoot + loot.GetName ());
			}
		}
    }

	#region singleton
	static private Player _instance;
	static public Player Instance {
		get {
			if ( _instance == null ) {
				_instance = GameObject.FindObjectOfType(typeof(Player)) as Player;
			}
			return _instance;
		}
	}
	#endregion
}

