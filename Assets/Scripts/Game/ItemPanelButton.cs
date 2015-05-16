using UnityEngine;
using System.Collections;

public class ItemPanelButton : MonoBehaviour {
	public ItemPanelScript owner;
	public GameObject buildingPrefab;
	
	void Start()
	{
		owner = transform.parent.GetComponent<ItemPanelScript>() as ItemPanelScript;
	}
	
	public void ButtonClicked()
	{
		//owner.ChildButtonClicked();
	}
	
}