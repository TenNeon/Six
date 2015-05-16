using UnityEngine;
using System.Collections;

public class CommandCard : MonoBehaviour {
	Rect cardPosition = new Rect(Screen.width-200, Screen.height-200,200,200);
	public int selGridInt = 0;
	public string[] selStrings = new string[] {"Move", "Stop", "Assign", ""};


	// Use this for initialization
	void Start () {
	
	}

	void Update()
	{
		if(selGridInt != 0)
		{
			Debug.Log(selGridInt);
			selGridInt = 0;
		}
	}

	void OnGUI()
	{
		GUILayout.BeginArea(cardPosition);
		selGridInt = GUI.SelectionGrid(new Rect(0,0,200,200), selGridInt, selStrings, 2);
		GUILayout.EndArea();
	}

	void OnScreenResize()
	{
		cardPosition = new Rect(Screen.width-200, Screen.height-200,200,200);
	}
}