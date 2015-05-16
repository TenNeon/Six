using UnityEngine;
using System.Collections;

public class ZoomControl : MonoBehaviour {

	float minSize = 1;
	float maxSize = 20;
	float zoomSensitivity = 4f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetAxis("Mouse ScrollWheel") != 0)
		{
			Camera.main.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * zoomSensitivity;

			if(Camera.main.orthographicSize < minSize)
			{
				Camera.main.orthographicSize = minSize;
			}
			if(Camera.main.orthographicSize > maxSize)
			{
				Camera.main.orthographicSize = maxSize;
			}
		}
	}
}
