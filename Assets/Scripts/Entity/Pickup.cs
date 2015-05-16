using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {
	public string type = "nothing";
	public bool alive = true;
	float maxAge = 10f;
	public bool claimed = false;

	void Start()
	{
		Invoke("DestroySelf",maxAge);
	}

	public string ConvertToCargo()
	{
		if(alive)
		{
			StartCoroutine("DestroySelf");
		}
		return type;
	}

	IEnumerator DestroySelf()
	{
		alive = false;
		float deathTime = 0.7f;
		float startTime = Time.time;

		var collider = GetComponent("CircleCollider2D") as CircleCollider2D;
		if(collider != null)
		{
			collider.enabled = false;
		}
		
		while (Time.time - startTime < deathTime) 
		{
			transform.localScale = transform.localScale * 0.99f;
			GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, Color.clear, 0.1f);
			yield return null;
		}
		Destroy (this.gameObject);
	}


}
