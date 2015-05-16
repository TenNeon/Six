using UnityEngine;
using System.Collections;

public class PauseMenu : MonoBehaviour {
	public bool showMenu = false;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnPause()
	{
		showMenu = true;
	}

	void OnResume()
	{
		showMenu = false;
	}

}
