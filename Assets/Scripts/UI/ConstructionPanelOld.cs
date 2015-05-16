using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConstructionPanelOld : MonoBehaviour {
	public List<GameObject> buildingPrefabs;
	GameObject selectedBuilding;
	int itemSize = 100;
	Rect windowRect;

	bool isPlacingBuilding = false;

	void Start()
	{
		Resize();
	}

	void OnWindowResize()
	{
		Resize();
	}

	void Resize()
	{
		windowRect = new Rect((Screen.width - buildingPrefabs.Count*itemSize)/2, Screen.height-130, 120, 100);
	}

	void OnGUI()
	{
		if(Event.current.type == EventType.layout)
		{
			windowRect = GUILayout.Window(0, windowRect, DoMyWindow, "Buildings", GUILayout.Width(150));
		}

		if(Event.current.type == EventType.MouseDown && Input.GetMouseButtonDown(0))
		{
			if (isPlacingBuilding) 
			{
				SpawnBuilding(selectedBuilding);
			}
		}
	}

	void DoMyWindow(int windowID)
	{
		GUIStyle buttonStyle = GUI.skin.GetStyle("Button");
		buttonStyle.wordWrap = true;
		GUILayout.BeginHorizontal ();
		for(int i = 0; i< buildingPrefabs.Count; i++)
		{
			SpriteRenderer renderer = buildingPrefabs[i].GetComponent("SpriteRenderer") as SpriteRenderer;

			if(renderer.sprite.texture != null)
			{
				if(GUILayout.Button(renderer.sprite.texture, 
				                    GUILayout.Width(itemSize),
				                    GUILayout.Height(itemSize),
				                    GUILayout.MinWidth(itemSize),
				                    GUILayout.MinHeight(itemSize))
				   && !isPlacingBuilding
				 )
				{
					selectedBuilding = buildingPrefabs[i];
					isPlacingBuilding = true;
					Debug.Log ("start place");
				}
			}
			else
			{
				Debug.LogError("No sprite/texture found for " + buildingPrefabs[i].name);
			}
		}
		GUILayout.EndHorizontal ();
	}

	void SpawnBuilding(GameObject building)
	{
		Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pos.z = 0;

		Instantiate(building, pos, Quaternion.identity);
		building = null;
		isPlacingBuilding = false;
	}
	
}
