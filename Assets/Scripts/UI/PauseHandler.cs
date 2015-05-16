using UnityEngine;
using System.Collections;

public class PauseHandler : MonoBehaviour {

	void OnGUI()
	{
		if(Event.current.type == EventType.layout){
			if(Input.GetKeyDown("escape"))
			{
				Object[] objects = FindObjectsOfType (typeof(GameObject));
				string message;
				if(Time.timeScale != 0)
				{
					message = "OnPause";
				}
				else {
					message = "OnResume";
				}
				
				foreach (GameObject go in objects) {
					go.SendMessage (message, SendMessageOptions.DontRequireReceiver);
				}
				//showMenu = !showMenu;
			}
		}
	}

	public void OnPause()
	{
		Time.timeScale = 0;
	}

	public void OnResume()
	{
		Time.timeScale = 1;
	}
}
