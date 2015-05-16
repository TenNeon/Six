using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EnemyScanner : MonoBehaviour {
	List<Enemy> targets = new List<Enemy>();
	// Use this for initialization
	void Start () {
		InvokeRepeating("CleanUp",0,0.5f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		var enemy = col.gameObject.GetComponent("Enemy") as Enemy;
		if(enemy != null)
		{
			targets.Add(enemy);
		}
	}

	void OnTriggerExit2D(Collider2D col)
	{
		var enemy = col.gameObject.GetComponent("Enemy") as Enemy;
		if(enemy != null)
		{
			targets.Remove(enemy);
		}
	}

	void CleanUp()
	{
		for(int i = 0; i<targets.Count; i++)
		{
			if(targets[i]==null || !targets[i].alive)
			{
				targets.RemoveAt(i);
			}
		}
	}

	public Enemy PickTarget()
	{
		CleanUp();
		if(targets.Count > 0)
		{
			targets.Sort (CompareTargetsByDistance);
			return targets[0];
		}
		else{
			return null;
		}
	}

	private int CompareTargetsByDistance(Enemy x, Enemy y)
	{
		var xDist = Vector3.Distance(x.transform.position, transform.position);
		var yDist = Vector3.Distance(y.transform.position, transform.position);
		
		return xDist.CompareTo(yDist);
	}

}
