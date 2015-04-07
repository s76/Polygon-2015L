using UnityEngine;
using System.Collections;

public class ItemScript : MonoBehaviour{
	private Item item;

	public ItemScript(Item item){
		this.item = item;
	}

	void Start(){
		this.item = new Gun1 ("przedmiot dwureczny", true);
	}
	public Item GetItem(){
		return item;
	}

	//sprawia ze obiekt "znika" ze swiata gry na czas przebywania w ekwipunku
	public void DisableAsPhysicalObject(){
		GetComponent<SphereCollider>().enabled = false;
		GetComponent<MeshRenderer> ().enabled = false;
	}

    public void EnableAsPhysicalObject(){
        GetComponent<SphereCollider>().enabled = true;
        GetComponent<MeshRenderer>().enabled = true;
    }
}

public class Item {
	protected bool isDoubleHanded;
	protected string name;
    protected ItemScript parentScript;
	
	public Item( string name, bool isDoubleHanded) {
        //this.parentScript = parentScript;
		this.name = name;
		this.isDoubleHanded = isDoubleHanded;
	}
	
	public void onUse(){	    
	}
	
	public bool IsDoubleHanded() {
		return isDoubleHanded;
	}
	
	public string GetName() {
		return name;
	}
}

public class Gun1 : Item{
	
	public Gun1(string name, bool isDoubleHanded) : base(name, isDoubleHanded){
	}
	
	public void onUse() {
		Debug.Log("GUn1");
	}
}

public class Gun2 : Item{
	
	public Gun2(string name, bool isDoubleHanded) : base(name, isDoubleHanded){
	}
	
	public void onUse(){
		Debug.Log("GUn2");
	}
} 