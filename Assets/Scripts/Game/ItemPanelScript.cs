using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemPanelScript : MonoBehaviour {
	public GameObject buttonPrefab;
	Image itemsListPanel;
	int buttonsPerRow = 4;

	// Use this for initialization
	void Start () {
	
	}
	
	public void UpdateItemsPanel ()
	{
		Debug.Log ("Updating items panel.");
		Transform newButton;
		var itemPrefabs = ItemTypeKeeper.main.GetItemPrefabs();
		for(int i = 0; i < itemPrefabs.Count; i++)
		{
			newButton = (Instantiate(buttonPrefab) as GameObject).transform;
			newButton.SetParent(this.transform);
			var prefabSprite = itemPrefabs[i].GetComponent<SpriteRenderer>().sprite;
			newButton.transform.name = itemPrefabs[i].transform.name + " button";


			//set the button sprite to the item sprite
			foreach (Transform child in newButton.transform) {
				if(child.name == "Image")
				{
					Image buttonImage = child.GetComponent<Image>() as Image;
					buttonImage.sprite = prefabSprite;

					break;
				}
			}

			//place the button in its spot on the panel
			var rectTransform = newButton.GetComponent<RectTransform>();
			float buttonSize = 1f;
			float buttonSeparation = 2.1f;
			rectTransform.offsetMin = new Vector2(
				((buttonSize+buttonSeparation)*(i%buttonsPerRow)) + buttonSeparation/2				
				,-2-(i-(i%buttonsPerRow)));
			rectTransform.offsetMax = new Vector2(
				((buttonSize+buttonSeparation)*(i%buttonsPerRow)) + (buttonSize+buttonSeparation/2)	
				,-1-(i-(i%buttonsPerRow)));
			rectTransform.anchorMin = new Vector2(0,1);
			rectTransform.anchorMax = new Vector2(0,1);

			
			//set the prefab for the button
			//ConstructionPanelButton newButtonScript = newButton.GetComponent<ConstructionPanelButton>() as ConstructionPanelButton;
			//newButtonScript.buildingPrefab = buildingPrefabs[i];
		}
	}
}
