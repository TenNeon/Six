using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {
	public float keyScrollSpeed = .5f;
	public float edgeScrollSpeed = .2f;
	public Rect bounds;
	public float edgeSize = 50f;

	// Use this for initialization
	void Start () {
		if (bounds == null) {
			bounds = new Rect(0,0,50,20);
		}

	}
	
	// Update is called once per frame
	void Update () {
		var pos = transform.position;
		pos = KeyMovement (pos);
		pos = EdgeMovment (pos);

		transform.position = pos;
		CheckBounds();

	}

	Vector3 KeyMovement (Vector3 pos)
	{
		if (Input.GetAxis ("Horizontal") > 0)
		{
			pos.x = pos.x + keyScrollSpeed;
		}
		if (Input.GetAxis ("Horizontal") < 0)
		{
			pos.x = pos.x - keyScrollSpeed;
		}
		if (Input.GetAxis ("Vertical") > 0)
		{
			pos.y = pos.y + keyScrollSpeed;
		}
		if (Input.GetAxis ("Vertical") < 0)
		{
			pos.y = pos.y - keyScrollSpeed;
		}
		return pos;
	}

	Vector3 EdgeMovment(Vector3 pos)
	{
		if (   Input.mousePosition.x > Screen.width - edgeSize && Input.mousePosition.x < Screen.width){
			pos.x = pos.x + edgeScrollSpeed;
		}
		if (   Input.mousePosition.y > Screen.height - edgeSize && Input.mousePosition.y < Screen.height){
			pos.y = pos.y + edgeScrollSpeed;
		}
		if (Input.mousePosition.y < edgeSize && Input.mousePosition.y > 0)
		{
			pos.y = pos.y - edgeScrollSpeed;
		}
		if (Input.mousePosition.x < edgeSize && Input.mousePosition.x > 0)
		{
			pos.x = pos.x - edgeScrollSpeed;
		}
		return pos;
	}

	void CheckBounds()
	{
		var pos = transform.position;
		if(pos.x > bounds.x+bounds.width)
		{
			pos.x = bounds.x+bounds.width;
		}
		if(pos.x < bounds.x)
		{
			pos.x = bounds.x;
		}
		if(pos.y > bounds.y+bounds.height)
		{
			pos.y = bounds.y+bounds.height;
		}
		if(pos.y < bounds.y)
		{
			pos.y = bounds.y;
		}
		
		transform.position = pos;
	}

}
