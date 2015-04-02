using UnityEngine;
using System.Collections;

public class Item {

    private bool isDoubleHanded;
    private string name;

    public Item(string name, bool isDoubleHanded) {
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