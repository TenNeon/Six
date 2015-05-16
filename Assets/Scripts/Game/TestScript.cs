using UnityEngine;
using System.Collections;

public class TestScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Fire1"))
		{
			Debug.Log("Click");
			var pos = Input.mousePosition;
			pos = Camera.main.ScreenToWorldPoint(pos);
			Ground.ground.AddMaterial("metal",10,new Vector2(pos.x,pos.y));
		}
	}

}
