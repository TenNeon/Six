using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Harvester : Building {
	public int harvestRadius = 10;
	int currentHarvestRadius = 0;
	public int harvestAmount = 3;
	public float harvestRate = 1f;
	public int oreStorage = 0;
	int amountPerLoad = 50;
	float lastHarvestTime = 0f;
	int lastHarvestAmount = 0;
	public int energy = 0;
	int minimumEnergyBeforeDischarge = 450;
	bool isDischargingCell = false;

	// Use this for initialization
	void Start () {
		BuildingStart();
	}

	void Tick()
	{
		if(!isDischargingCell && energy < minimumEnergyBeforeDischarge)
		{
			var fullCells = resourceProfiles.Find(x => x.name == "Energy Cell (full)").GetComponent("ResourceProfile") as ResourceProfile;
			if(fullCells!=null && fullCells.amount > 0)
			{
				StartCoroutine("DischargeCell");
			}
		}

		if (oreStorage >= 100) 
		{
			var ore = resourceProfiles.Find(x => x.name == "Ore").GetComponent("ResourceProfile") as ResourceProfile;
			
			if(ore != null)
			{
				ore.amount += 1;
				oreStorage -= 100;
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if(Time.time > lastHarvestTime + (1/harvestRate))
		{
			Harvest ();
			lastHarvestTime = Time.time;
		}
	}

	void Harvest()
	{
		if(energy >= currentHarvestRadius * currentHarvestRadius)
		{
			energy -= currentHarvestRadius * currentHarvestRadius;
		}
		else
		{
			Debug.Log ("Harvester needs energy: " + (currentHarvestRadius * currentHarvestRadius * 3 - energy));
			return;
		}
		var pos = transform.position;
		var r = currentHarvestRadius;

		lastHarvestAmount = 0;
		for(int y=-r; y<=r; y++){
			for(int x=-r; x<=r; x++){
				if(x*x+y*y <= r*r+r*0.8f){

					if(Random.Range(0f,1f) > ((float)(currentHarvestRadius)/(float)harvestRadius))
					{
						lastHarvestAmount += Ground.ground.AddMaterial("Ore",-harvestAmount,new Vector2(pos.x,pos.y),new Vector2(x,y));
					}
				}
			}
		}
		oreStorage += lastHarvestAmount;
		currentHarvestRadius = Random.Range(1,harvestRadius);
	}	

	IEnumerator DischargeCell()
	{
		ResourceProfile fullCells = resourceProfiles.Find(x => x.name == "Energy Cell (full)").GetComponent("ResourceProfile") as ResourceProfile;
		ResourceProfile emptyCells = resourceProfiles.Find(x => x.name == "Energy Cell (empty)").GetComponent("ResourceProfile") as ResourceProfile;
		
		fullCells.amount -= 1;
		isDischargingCell = true;
		for (int i = 0; i < 100; i++) {
			energy += 1;
			yield return new WaitForSeconds(0.02F);
		}
		isDischargingCell = false;
		emptyCells.amount += 1;
	}
	
    //void OnGUI()
    //{
    //    if(SelectionBox.main.selectedObjects.Contains(this.gameObject))
    //    {
    //        windowRect = GUILayout.Window(3, windowRect, DoMyWindow, "Ore Harvester");
    //    }
    //}
	
	void DoMyWindow(int windowID)
	{
		var fullCells = resourceProfiles.Find(x => x.name == "Energy Cell (full)").GetComponent("ResourceProfile") as ResourceProfile;
		var ore = resourceProfiles.Find(x => x.name == "Ore").GetComponent("ResourceProfile") as ResourceProfile;
		var emptyCells = resourceProfiles.Find(x => x.name == "Energy Cell (empty)").GetComponent("ResourceProfile") as ResourceProfile;

		GUIStyle buttonStyle = GUI.skin.GetStyle("Button");
		buttonStyle.wordWrap = true;
		GUILayout.Label ("Energy: " + energy);
		GUILayout.Label ("Ore: " + oreStorage);
		GUILayout.Label ("Inputs: ");
		GUILayout.Label ("Energy Cell (full): " + fullCells.amount);
		GUILayout.Label ("Outputs: ");
		GUILayout.Label ("Ore: " + ore.amount);
		GUILayout.Label ("Energy Cell (empty): " + emptyCells.amount);

		GUI.DragWindow();
	}
}
