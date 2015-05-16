using UnityEngine;
using System.Collections;
using System;
using System.IO;

public class WriteToFile : MonoBehaviour {

	// Use this for initialization
	void Start () {
		string fileName = "WriteFile Test " + DateTime.Now.ToLongDateString()+".txt";
		StreamWriter streamWriter;
		if (File.Exists(fileName))
		{
			streamWriter = File.AppendText(fileName);
		}
		else{
			streamWriter = File.CreateText(fileName);
		}
		streamWriter.WriteLine ("File written at " +DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
		streamWriter.Close();
	}
}