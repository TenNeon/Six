using UnityEngine;
//using System;
using System.Collections;
using System.Collections.Generic;

public class Building : MonoBehaviour, IWorkerOwner{
	internal List<ResourceProfile> resourceProfiles = new List<ResourceProfile>();
	public bool hovered = false;
	internal Rect windowRect = new Rect(0, 0, 180, Screen.height);

	public List<Worker> workers = new List<Worker>();

	Color baseColor = Color.white;
	
	protected void BuildingStart()
	{
		float randomStart = Random.Range(0,2f);
		InvokeRepeating("MaintainStocks",0,1f);
		randomStart = Random.Range(0,2f);
		InvokeRepeating("Tick",randomStart,1f);
		ClaimAssignedWorkers();
	}

	// When the script starts, workers assigned to this building may not have
	// this building set as their owner. This sets them.
	void ClaimAssignedWorkers()
	{
		for (int i = 0; i < workers.Count; i++) {
			workers[i].owner = this;
		}
	}

	void MaintainStocks()
	{
		for(int i = 0; i < resourceProfiles.Count; i++)
		{
			var profile = resourceProfiles[i];
			profile.Maintain();
		}
	}

	void OnMouseEnter()
	{
		hovered = true;
		if(ResourceProfile.newLinkResourceType != "no resource type")
		{
			var renderer = GetComponent("SpriteRenderer") as SpriteRenderer;
			Color sameTypeColor = new Color(0.5f,1f,0.5f);
			
			var containsResourceType = resourceProfiles.Find(x => x.resourceType == ResourceProfile.newLinkResourceType);
			
			if(containsResourceType != null)
			{
				renderer.color = sameTypeColor;
			}
			else 
			{
				renderer.color = Color.red;	
			}
		}

	}

	void OnMouseUp()
	{
		if(ResourceProfile.newLinkResourceType != "no resource type")
		{
			ResetColor();
		}
	}

	void OnMouseExit()
	{
		hovered = false;
		if(ResourceProfile.newLinkResourceType != "no resource type")
		{
			ResetColor();
		}
	}

	void ResetColor()
	{
		var renderer = GetComponent("SpriteRenderer") as SpriteRenderer;
		
		renderer.color = baseColor;
	}

	public bool AssignWorker(Worker w)
	{
		if (!workers.Contains(w) )
		{
			workers.Add(w);
			return true;
		}
		return false;
	}

	public void UnassignWorker(Worker w)
	{
		if (workers.Contains(w) )
		{
			workers.Remove(w);
		}
	}

	public void AssignWorkersToOrder(Order o)
	{
		for(int i = 0; i < workers.Count; i++)
		{
			if(o.amount - o.assignedWorkers.Count <= 0)
			{
				return;
			}
			if(workers[i].AssignOrder(o))
			{
				break;
			}
		}
	}

	public Vector3 IdlePosition()
	{
		return transform.position;
	}
}
