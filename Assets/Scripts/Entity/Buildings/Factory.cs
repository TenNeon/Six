using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Factory : Building {
	int energy = 0;
	int energyReq = 200;
	int minimumEnergyBeforeDischarge = 250;
	bool isDischargingCell = false;
	bool buildWorkers = false;
	//bool buildMunitions = false;
	bool buildEnergyCells = false;
	public Transform workerPrefab;

	public int workerMetalCost = 3;
	public int workerEnergyCellCost = 1;
	public int energyCellMetalCost = 1;

	void Start () {
		InvokeRepeating("Tick",0f,5f);
		float randomStart = Random.Range(0,2f);
		InvokeRepeating("MaintainStocks",randomStart,2f);
	}

	void Tick()
	{
//		if(buildMunitions && energy >= energyReq)
//		{
//			var munitions = resourceProfiles.Find(x => x.resourceType == "Munitions").GetComponent("ResourceProfile") as ResourceProfile;
//			var metal = resourceProfiles.Find(x => x.resourceType == "Metal").GetComponent("ResourceProfile") as ResourceProfile;
//
//			if(munitions!=null && metal != null && metal.AvailableAmount > 0)
//			{
//				munitions.amount += 1;
//				metal.amount -= 1;
//				energy -= energyReq;
//			}
//		}
		if(buildEnergyCells && energy >= energyReq)
		{
			var cells = resourceProfiles.Find(x => x.resourceType == "Energy Cell (empty)").GetComponent("ResourceProfile") as ResourceProfile;
			var metal = resourceProfiles.Find(x => x.resourceType == "Metal").GetComponent("ResourceProfile") as ResourceProfile;
			
			if(cells!=null && metal != null && metal.AvailableAmount > energyCellMetalCost)
			{
				cells.amount += 1;
				metal.amount -= 1;
				energy -= energyReq;
			}
		}
		else if(buildWorkers && energy >= energyReq)
		{
			var fullCells = resourceProfiles.Find(x => x.name == "Energy Cell (full)").GetComponent("ResourceProfile") as ResourceProfile;
			var metal = resourceProfiles.Find(x => x.name == "Metal").GetComponent("ResourceProfile") as ResourceProfile;

			if(fullCells.amount >= workerEnergyCellCost && metal.amount >= workerMetalCost)
			{
				Debug.Log ("build worker");
				fullCells.amount -= workerEnergyCellCost;
				metal.amount -= workerMetalCost;
				energy -= energyReq;
				Instantiate(workerPrefab, transform.position, transform.rotation);
			}
		}
		if(!isDischargingCell && energy < minimumEnergyBeforeDischarge)
		{
			var fullCells = resourceProfiles.Find(x => x.name == "Energy Cell (full)").GetComponent("ResourceProfile") as ResourceProfile;
			if(fullCells!=null && fullCells.amount > 0)
			{
				StartCoroutine("DischargeCell");
			}
		}
	}
	
	void OnGUI()
	{
        //if(SelectionBox.main.selectedObjects.Contains(this.gameObject))
        //{
        //    windowRect = GUILayout.Window(2, windowRect, DoMyWindow, "Factory");
        //}
	}
	
	void DoMyWindow(int windowID)
	{
		var fullCells = resourceProfiles.Find(x => x.name == "Energy Cell (full)").GetComponent("ResourceProfile") as ResourceProfile;
		var metal = resourceProfiles.Find(x => x.name == "Metal").GetComponent("ResourceProfile") as ResourceProfile;
		var emptyCells = resourceProfiles.Find(x => x.name == "Energy Cell (empty)").GetComponent("ResourceProfile") as ResourceProfile;
		var munitions = resourceProfiles.Find(x => x.name == "Munitions").GetComponent("ResourceProfile") as ResourceProfile;

		GUIStyle buttonStyle = GUI.skin.GetStyle("Button");
		buttonStyle.wordWrap = true;
		GUILayout.Label ("Inputs: ");
		GUILayout.Label ("Metal: " + metal.amount);
		GUILayout.Label ("Energy Cell (full): " + fullCells.amount);
		GUILayout.Label ("Outputs: ");
		GUILayout.Label ("Munitions: " + munitions.amount);
		GUILayout.Label ("Energy Cell (empty): " + emptyCells.amount);
		GUILayout.Label ("Costs: ");
		GUILayout.Label ("Worker: " + workerMetalCost + " metal, " + workerEnergyCellCost + " charged energy cells.");
		GUILayout.Label ("Energy Cell: " + energyCellMetalCost + " metal");

		string buttonText = "Build Workers";
		if(buildWorkers)
		{
			buttonText = "Stop Building Workers";
		}
		if(GUILayout.Button(buttonText,buttonStyle))
		{
			buildWorkers = !buildWorkers;
		}

		buttonText = "Build Energy Cells";
		if(buildEnergyCells)
		{
			buttonText = "Stop Building Energy Cells";
		}
		if(GUILayout.Button(buttonText,buttonStyle))
		{
			buildEnergyCells = !buildEnergyCells;
		}
//		buttonText = "Build Munitions";
//		if(buildMunitions)
//		{
//			buttonText = "Stop Building Munitions";
//		}
//		if(GUILayout.Button(buttonText,buttonStyle))
//		{
//			buildMunitions = !buildMunitions;
//		}
		GUI.DragWindow();
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
}
