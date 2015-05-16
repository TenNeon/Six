using UnityEngine;
using System.Collections;

public class Redirector : MonoBehaviour {
	public Vector2 origin;
	public Vector2 size = new Vector2(.1f,.1f);
	public float angle = 0;
	public Vector2 direction = new Vector2(1,0);
	public float distance = 3f;
	public int layerMask = 17;//17 - items
	public Vector2 pushDistancePerSecond = new Vector2(1f, 0f);
	// Use this for initialization
	void Start () {
		origin = new Vector2(transform.position.x, transform.position.y);
	}

	// Update is called once per frame
	void Update () {
		//Attempt1 ();
		Attempt2 ();
	}


	void Attempt1 ()
	{
		origin = new Vector2 (transform.position.x, transform.position.y);
		var hits = Physics2D.RaycastAll (origin, direction, distance);
		//Debug.Log (Time.time + ": " + hits.Length);
		for (int i = 0; i < hits.Length; i++) {
			//Debug.Log(i+ ": " + hits[i].transform.name);
			//			var pos = hits[i].transform.position;
			//			pos.x += 0.03f;
			//			hits[i].transform.position = pos;
			hits [i].transform.GetComponent<Rigidbody2D>().AddForce (new Vector2 (8f, 0));
		}
	}

	void Attempt2 ()
	{
		origin = new Vector2 (transform.position.x, transform.position.y);
		var hits = Physics2D.BoxCastAll(origin, size, angle, direction, distance);
		for (int i = 0; i < hits.Length; i++) {
			//hits [i].transform.rigidbody2D.AddForce (new Vector2 (8f, 0));
				//Debug.Log ("Hit " + hits[i].transform.name);
				var pos = hits [i].transform.position;
				var pushV3 = new Vector3(pushDistancePerSecond.x*Time.deltaTime, pushDistancePerSecond.y*Time.deltaTime, 0);
				pos += pushV3;
				hits[i].transform.position = pos;
		}
	}
}
