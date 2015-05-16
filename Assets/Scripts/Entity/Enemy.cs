using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Enemy : MonoBehaviour {
	float age = 0;
	public float maxAge = 60f;
	public float hitPoints = 2f;
	public float speed = .5f;
	public bool alive = true;
	public float armor = 0;
	public List<Transform> potentialDrops;
	Color color = Color.red;
	// Use this for initialization
	void Start () {
		StartCoroutine("Spawn");
	}
	
	// Update is called once per frame
	void Update () {
		age += Time.deltaTime;
		hitPoints += hitPoints * Time.deltaTime * 0.03f;
		if(age >= maxAge)
		{
			StartCoroutine("Die");
		}

		MoveTowardCenter();
	}

	public void Hit(float damage)
	{
		hitPoints -= damage -armor;
		armor *= 0.95f;
		if(hitPoints <= 0)
		{
			Drop();
			StartCoroutine("Die");
		}
		else {
			StartCoroutine("Flash");
			//StartCoroutine("Jump");
		}
	}

	IEnumerator Die()
	{
		alive = false;
		float deathTime = 0.7f;
		float startTime = Time.time;

		var collider = GetComponent("CircleCollider2D") as CircleCollider2D;
		collider.enabled = false;
		while (Time.time - startTime < deathTime) 
		{
			transform.localScale = transform.localScale * 1.01f*Time.deltaTime;
			GetComponent<Renderer>().material.color = Color.Lerp(GetComponent<Renderer>().material.color, Color.clear, 0.1f);
			yield return null;
		}
		Destroy (this.gameObject);
	}

	IEnumerator Flash()
	{
		var sprite = GetComponent("SpriteRenderer") as SpriteRenderer;
		sprite.color = Color.white;
		var startTime = Time.time;
		var flashDuration = 0.4f;
		while(Time.time < startTime + flashDuration)
		{
			sprite.color = Color.Lerp(sprite.color, color, 0.1f);
			yield return null;
		}
		sprite.color = color;
	}

	IEnumerator Jump()
	{
		Vector2 jump = Random.insideUnitCircle*0.05f;
		transform.position += new Vector3(jump.x,jump.y,0);
		yield return null;
	}

	IEnumerator Spawn()
	{
		float animationTime = 0.9f;
		float startTime = Time.time;
		var startScale = transform.localScale;
		transform.localScale = Vector3.Lerp(transform.localScale,Vector3.zero,0.5f);
		var collider = GetComponent("CircleCollider2D") as CircleCollider2D;
		collider.enabled = false;

		while (Time.time - startTime < animationTime) {
			transform.localScale = Vector3.Lerp(transform.localScale,startScale,0.1f);
			yield return null;
		}

		collider.enabled = true;
	}

	void MoveTowardCenter()
	{
		var pos = transform.position;
		//transform.position = Vector3.Lerp(pos, Vector3.zero, 0.01f);
		transform.position = Vector3.MoveTowards(pos, Vector3.zero, speed * Time.deltaTime);
	}

	void OnGUI()
	{
		if(!alive){return;}
		var pos = Camera.main.WorldToScreenPoint(transform.position);
		var text = hitPoints.ToString("F1") + "("+ armor.ToString("F2")+")";
		GUIStyle style = new GUIStyle();
		style.normal.textColor = Color.black;
		GUI.Label (new Rect (pos.x-14, Camera.main.pixelHeight-pos.y+10, 100, 30), text, style);
	}

	void Drop()
	{
		if(potentialDrops.Count == 0){return;}
		float dropPercent = 0.3f;
		//decide if something should drop
		float shouldDroproll = Random.Range(0,1f);
		//if so, decide what should drop
		if (shouldDroproll < dropPercent)
		{
			int whichDropRoll = Random.Range(0,potentialDrops.Count);
			var position = transform.position;
			var rotation = transform.rotation;
			Instantiate(potentialDrops[whichDropRoll],position,rotation);
		}

	}
}
