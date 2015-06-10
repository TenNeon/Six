using UnityEngine;
using System.Collections;

public class Smelter : Building {
	public int energy = 0;
	int energyReq = 500;
	int energyPerCell = 100;
	int orePerUnit = 100;
	int metalPerUnit = 100;
	int slagPerUnit = 500;
	public float ore = 0;
	public float metal = 0;
	public float slag = 0;
	public float orePurity = 0.4f;

	bool isDischargingCell = false;
	bool isUnloadingOre = false;
	bool isLoadingMetal = false;
	bool isLoadingSlag = false;

	int loadSize = 20;

	enum State {Idle, Smelting};
	State state = State.Idle;
	// Use this for initialization
	void Start () {
		BuildingStart();
	}

	#region Overhead stuff
	void Tick()
	{
		ProcessOre();

		if(!isDischargingCell && energy < energyReq)
		{
			var fullCells = resourceProfiles.Find(x => x.name == "Energy Cell (full)").GetComponent("ResourceProfile") as ResourceProfile;
			if(fullCells!=null && fullCells.amount > 0)
			{
				StartCoroutine("DischargeCell");
			}
		}
		if(!isUnloadingOre && ore < 100)
		{
			var oreUnits = resourceProfiles.Find(x => x.name == "Ore").GetComponent("ResourceProfile") as ResourceProfile;
			if(oreUnits!=null && oreUnits.amount > 0)
			{
				StartCoroutine("UnloadOre");
			}
		}
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

	IEnumerator UnloadOre()
	{
		ResourceProfile oreUnits = resourceProfiles.Find(x => x.name == "Ore").GetComponent("ResourceProfile") as ResourceProfile;

		oreUnits.amount -= 1;
		isUnloadingOre = true;
		for (int i = 0; i < orePerUnit; i++) {
			ore += 1;
			yield return new WaitForSeconds(0.02F);
		}

		isUnloadingOre = false;
	}

	IEnumerator LoadMetal()
	{
		ResourceProfile metalUnits = resourceProfiles.Find(x => x.name == "Metal").GetComponent("ResourceProfile") as ResourceProfile;

		isLoadingMetal = true;
		for (int i = 0; i < metalPerUnit; i++) {
			metal -= 1;
			yield return new WaitForSeconds(0.02F);
		}
		metalUnits.amount += 1;
		isLoadingMetal = false;	
	}

	IEnumerator LoadSlag()
	{
		ResourceProfile slagUnits = resourceProfiles.Find(x => x.name == "Slag").GetComponent("ResourceProfile") as ResourceProfile;
		
		isLoadingSlag = true;
		for (int i = 0; i < slagPerUnit; i++) {
			slag -= 1;
			yield return new WaitForSeconds(0.02F);
		}
		slagUnits.amount += 1;
		isLoadingSlag = false;	
	}


#endregion

	void ProcessOre()
	{
		energy = Mathf.RoundToInt(energy*0.999f);

		Smelt();

		Package();	
	}

	//convert furnace content into slag
	void Smelt()
	{
		int smeltingRate = 10;
		if(energy < smeltingRate)
		{
			return;
		}
		if(ore >= smeltingRate)
		{
			metal += smeltingRate*orePurity;
			energy -= smeltingRate;

			slag += smeltingRate*(1-orePurity);
			ore -= 10;
		}
		else
		{
			energy -= smeltingRate;
			metal += ore*orePurity;
			slag += ore*(1-orePurity);
			ore = 0;
		}
	}

	//package slag and metal for transport
	void Package()
	{
		if(!isLoadingMetal && metal >= metalPerUnit)
		{
			var metalUnits = resourceProfiles.Find(x => x.name == "Metal").GetComponent("ResourceProfile") as ResourceProfile;
			if(metalUnits!=null)
			{
				StartCoroutine("LoadMetal");
			}
		}
		if(!isLoadingSlag && slag >= slagPerUnit)
		{
			var slagUnits = resourceProfiles.Find(x => x.name == "Slag").GetComponent("ResourceProfile") as ResourceProfile;
			if(slagUnits!=null)
			{
				StartCoroutine("LoadSlag");
			}
		}
	}
    //void OnGUI()
    //{
    //    if(SelectionBox.main.selectedObjects.Contains(this.gameObject))
    //    {
    //        windowRect = GUILayout.Window(1, windowRect, DoMyWindow, "Smelter");
    //    }
    //}
	
	void DoMyWindow(int windowID)
	{
		GUIStyle buttonStyle = GUI.skin.GetStyle("Button");
		buttonStyle.wordWrap = true;
		var packagedMetal = resourceProfiles.Find(x => x.name == "Metal").GetComponent("ResourceProfile") as ResourceProfile;

		GUILayout.Label ("Energy: " + energy.ToString("F0"));
		GUILayout.Label ("Ore: " + ore);
		GUILayout.Label ("Metal: " + metal + " ("+packagedMetal.amount+")");
		GUILayout.Label ("Slag: " + slag);
		GUI.DragWindow();
	}
}
