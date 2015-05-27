using UnityEngine;
using System.Collections.Generic;
using System;

public class LootInventoryBuilder : MonoBehaviour
{

	private Dictionary<Type, int> dictionary = new Dictionary<Type, int>(); 
	private int totalCount = 0;

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public void PutLoot(AbsLoot loot){
		Type type = loot.GetType();
		if (dictionary.ContainsKey (type)) {
			dictionary [type] = dictionary [type]+1;
		} else {
			dictionary.Add(type, 1);
		}
		++totalCount;
		Debug.Log (totalCount + " loots");
		Debug.Log (dictionary);
	}

	public bool RemoveLoot(AbsLoot loot){
		Type type = loot.GetType();
		if (dictionary.ContainsKey (type)) {
			dictionary [type] = dictionary [type] - 1;
			--totalCount;
			return true;
		} 
		return false;
	}

	public int GetLootCount(AbsLoot loot){
		Type type = loot.GetType();
		return dictionary.ContainsKey(type) ? dictionary [type] : 0;
	}

	public int GetTotalLootCount(){
		return totalCount;
	}
}

