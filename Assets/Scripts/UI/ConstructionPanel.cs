using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ConstructionPanel : MonoBehaviour {

	public List<GameObject> buildingPrefabs;
	public GameObject panel; 
	public GameObject buttonPrefab;
	public GameObject ghostBuilding;
	
	bool isPlacingBuilding = false;
	bool canPlaceBuilding = false;

	GameObject selectedBuilding;

	void Start()
	{
		Transform newButton;

		for(int i = 0; i < buildingPrefabs.Count; i++)
		{
			newButton = (Instantiate(buttonPrefab) as GameObject).transform;

			var prefabSprite = buildingPrefabs[i].GetComponent<SpriteRenderer>().sprite;

			//set the button sprite to the building sprite
			foreach (Transform child in newButton.transform) {
				if(child.name == "Image")
				{
					Image buttonImage = child.GetComponent<Image>() as Image;
					buttonImage.sprite = prefabSprite;
					break;
				}
			}

			newButton.SetParent(gameObject.transform);//make the button a child of the panel
			//place the button in its spot on the panel
			var rectTransform = newButton.GetComponent<RectTransform>();
			rectTransform.offsetMin = new Vector2(5+50*i,-50);
			rectTransform.offsetMax = new Vector2(50+50*i,-5);
			rectTransform.anchorMin = new Vector2(0,1);
			rectTransform.anchorMax = new Vector2(0,1);

			//set the prefab for the button
			ConstructionPanelButton newButtonScript = newButton.GetComponent<ConstructionPanelButton>() as ConstructionPanelButton;
			newButtonScript.buildingPrefab = buildingPrefabs[i];
		}
	}

	void Update()
	{
		if(Input.GetMouseButtonDown(0))
		{
			if (isPlacingBuilding && canPlaceBuilding) 
			{
				Debug.Log ("spawn" + selectedBuilding.name);
				SpawnBuilding(selectedBuilding);
			}
		}
		if(isPlacingBuilding)
		{
			if(!canPlaceBuilding)
			{
				(ghostBuilding.GetComponent<SpriteRenderer>() as SpriteRenderer).color = new Color(1,.33f,.33f,.25f);
			}
			else 
			{
				(ghostBuilding.GetComponent<SpriteRenderer>() as SpriteRenderer).color = new Color(.33f,1,.45f,.25f);
			}
			var mousePos =  Camera.main.ScreenToWorldPoint( Input.mousePosition );
			ghostBuilding.transform.position = new Vector3(mousePos.x,mousePos.y, ghostBuilding.transform.position.z);
		}
	}

	public void ChildButtonClicked(GameObject clickedBuilding)
	{
		selectedBuilding = clickedBuilding;
		isPlacingBuilding = true;

		SetGhost(clickedBuilding);
		ShowGhost();
	}

	void SetGhost(GameObject clickedBuilding)
	{
		var buildingSprite = (clickedBuilding.GetComponent<SpriteRenderer>() as SpriteRenderer).sprite; //Get sprite of clickedBuilding
		(ghostBuilding.GetComponent<SpriteRenderer>() as SpriteRenderer).sprite = buildingSprite; //Set ghost's sprite to match
	}

	void ShowGhost()
	{
		ghostBuilding.transform.GetComponent<Renderer>().enabled = true;
	}

	void HideGhost()
	{
		ghostBuilding.transform.GetComponent<Renderer>().enabled = false;
	}

	public void PointerEnter()
	{
		canPlaceBuilding = false;
		//HideGhost();
	}

	public void PointerExit()
	{
		canPlaceBuilding = true;
//		if(isPlacingBuilding)
//		{
//			ShowGhost();
//		}
	}

	void SpawnBuilding(GameObject building)
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pos.z = 0;
		
		Instantiate(building, pos, Quaternion.identity);
		building = null;
		isPlacingBuilding = false;
		HideGhost();
	}

}
