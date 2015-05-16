using UnityEngine;
using System.Collections;

public class Debug_CommandCard : MonoBehaviour {
	string message = "";
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(message.Length != 0)
		{
			SendMessage();
			message = "";
		}
	}

	void OnGUI()
	{
//		if (GUI.Button(new Rect(10, 10, 100, 30), "Order All Stop"))
//			message = "OnStopCommand";
	}

	void SendMessage()
	{
		Object[] objects = FindObjectsOfType (typeof(GameObject));
		foreach (GameObject go in objects) {
			go.SendMessage (message, SendMessageOptions.DontRequireReceiver);
		}
	}
}
