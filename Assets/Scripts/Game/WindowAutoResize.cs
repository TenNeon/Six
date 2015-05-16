using UnityEngine;
using System.Collections;

public class WindowAutoResize : MonoBehaviour {
	int w;
	int h;
	// Use this for initialization

	void Start () {
		w = Screen.width;
		h = Screen.height;

		InvokeRepeating("Tick",0,0.1f);
	}
	
	// Update is called once per frame
	void Tick () {
		if (Screen.width != w || Screen.height != h) 
		{
			Object[] objects = FindObjectsOfType (typeof(GameObject));
			foreach (GameObject go in objects) {
				go.SendMessage ("OnWindowResize", SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
