using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class InputController : MonoBehaviour {
	List<string> commands = new List<string>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		commands.Clear();

		//each frame, assemble a list of current commands and send them to selected objects

		if(Input.GetButtonDown("Stop"))
		{
			commands.Add("OnStopCommand");
		}

		if(Input.GetButtonDown("Move"))
		{
			commands.Add("OnMoveCommand");
		}

		if(Input.GetButtonDown("Go Idle"))
		{
			commands.Add("OnGoIdleCommand");
		}

        //foreach (var item in SelectionBox.main.selectedObjects) {
        //    for (int i = 0; i < commands.Count; i++) {
        //        item.SendMessage (commands[i], SendMessageOptions.DontRequireReceiver);
        //    }
        //}
	}
}
