using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class PickupScanner : MonoBehaviour {
	List<Pickup> targets = new List<Pickup>();
	// Use this for initialization
	void Start () {
		InvokeRepeating("CleanUp",0,0.5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter2D(Collider2D col)
	{
		var pickup = col.gameObject.GetComponent("Pickup") as Pickup;
		if(pickup != null)
		{
			targets.Add(pickup);
		}
	}
	
	void OnTriggerExit2D(Collider2D col)
	{
		var pickup = col.gameObject.GetComponent("Pickup") as Pickup;
		if(pickup != null)
		{
			targets.Remove(pickup);
		}
	}
	
	void CleanUp()
	{
		for(int i = 0; i<targets.Count; i++)
		{
			if(targets[i]==null || !targets[i].alive || targets[i].claimed)
			{
				targets.RemoveAt(i);
			}
		}
	}
	
	public Transform PickTarget()
	{
		CleanUp();
		if(targets.Count > 0)
		{
			targets.Sort(CompareTargetsByDistance);
			return targets[0].transform;
		}
		else{
			return null;
		}
	}

	private int CompareTargetsByDistance(Pickup x, Pickup y)
	{
		var xDist = Vector3.Distance(x.transform.position, transform.position);
		var yDist = Vector3.Distance(y.transform.position, transform.position);
		
		return xDist.CompareTo(yDist);
	}
}
