using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DisplayPanelScript : MonoBehaviour {
	public Transform map;
	WorldData worldData;
	TileMapMouse mouse;
	public Text tilePositionText;
	public Text tileValueText;

	// Use this for initialization
	void Start () {
		worldData = map.GetComponent<WorldData>() as WorldData;
		mouse = map.GetComponent<TileMapMouse>() as TileMapMouse;
	}
	
	// Update is called once per frame
	void Update () {
		var pos = mouse.CurrentTileCoord;

		tileValueText.text = "Amount: " + worldData.GetTileValue((int)pos.x,(int)pos.y).ToString();
		tilePositionText.text = "Position: " + pos.x + "," + pos.y;
	}
}
