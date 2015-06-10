using UnityEngine;
using System.Collections;

public class PowerPlant : Building {
	public float energy = 0;
	public float energyPerSecond = 40f;
	int amountPerLoad = 100;
	float lastTick = 0f;
	// Use this for initialization
	void Start () {
		InvokeRepeating("SpawnOutput",0f,1f);
		BuildingStart();
	}
	
	// Update is called once per frame
	void Tick () {
		energy += energyPerSecond*(Time.time-lastTick);
		lastTick = Time.time;
	}

	void SpawnOutput()
	{
		if(energy >= amountPerLoad)
		{
			var fullCells = resourceProfiles.Find(x => x.name == "Energy Cell (full)").GetComponent("ResourceProfile") as ResourceProfile;
			var emptyCells = resourceProfiles.Find(x => x.name == "Energy Cell (empty)").GetComponent("ResourceProfile") as ResourceProfile;

			if ( emptyCells.amount > 0) 
			{
				emptyCells.amount -= 1;
				fullCells.amount += 1;
				energy -= amountPerLoad;
			}
		}
	}

    //void OnGUI()
    //{
    //    if(SelectionBox.main.selectedObjects.Contains(this.gameObject))
    //    {
    //        windowRect = GUILayout.Window(1, windowRect, DoMyWindow, "Power Plant");
    //    }
    //}
	
	void DoMyWindow(int windowID)
	{
		GUIStyle buttonStyle = GUI.skin.GetStyle("Button");
		buttonStyle.wordWrap = true;
		var emptyCells = resourceProfiles.Find(x => x.name == "Energy Cell (empty)").GetComponent("ResourceProfile") as ResourceProfile;
		var fullCells = resourceProfiles.Find(x => x.name == "Energy Cell (full)").GetComponent("ResourceProfile") as ResourceProfile;

		GUILayout.Label ("Energy: " + energy.ToString("F0"));
		GUILayout.Label ("Empty Cells: " + emptyCells.amount);
		GUILayout.Label ("Full Cells: " + fullCells.amount);

		GUILayout.Label ("Orders: ");
		for(int i = 0; i< resourceProfiles.Count; i++)
		{
			var strings = resourceProfiles[i].GetPlacedOrdersStrings();
			for (int j = 0; j < strings.Count; j++) {
				GUILayout.Label (strings[j]);
			}
		}

		GUI.DragWindow();
	}
}
