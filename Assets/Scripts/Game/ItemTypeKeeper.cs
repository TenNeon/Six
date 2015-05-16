using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ItemTypeKeeper : MonoBehaviour {
	public static ItemTypeKeeper main;
	public List<GameObject> itemPrefabs;
	// Use this for initialization
	void Start () {	
		if (ItemTypeKeeper.main == null) {
			ItemTypeKeeper.main = this;
		}
	}
	
	public List<GameObject> GetItemPrefabs()
	{
		Debug.Log ("Returning " + itemPrefabs.Count + " items.");
		return itemPrefabs;
	}
}
