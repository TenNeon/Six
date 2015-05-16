using UnityEngine;
using System.Collections;

public class ConstructionZone : Building {
	public GameObject buildingPrefab;
	public float buildTime = 30f;

	// Use this for initialization
	void Start () {
		BuildingStart();
		InvokeRepeating("CheckForComplete",0,0.1f);
	}

	void Tick()
	{

	}

	// Update is called once per frame
	void CheckForComplete () {
		for(int i = 0; i < resourceProfiles.Count; i++)
		{
			if (resourceProfiles[i].amount < resourceProfiles[i].capacity)
			{
				return;
			}
		}
		Complete ();
	}

	void Complete()
	{
		Instantiate(buildingPrefab, transform.position, Quaternion.identity);

		BroadcastMessage("OnBuildingRemoved");
		Destroy (this.gameObject);
	}
}
