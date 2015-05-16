using UnityEngine;
using System.Collections;

//[RequireComponent(typeof(TileMap))]
public class TileMapMouse : MonoBehaviour {
	public Transform tileSelectionObject;
	public Transform world;
	WorldData worldData;
	Vector3 previousTileCoord;
	Vector3 currentTileCoord;
	float tileSize = 1;

	void Start()
	{
		worldData = world.GetComponent("WorldData") as WorldData;
	}

	void Update () {
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hitInfo;

		if(GetComponent<Collider>().Raycast(ray, out hitInfo, Mathf.Infinity) )
		{
			int x = Mathf.FloorToInt(hitInfo.point.x / tileSize);
			int y = Mathf.FloorToInt(hitInfo.point.y / tileSize);

			currentTileCoord.x = x+tileSize/2;
			currentTileCoord.y = y+tileSize/2;
			currentTileCoord.z = 1;

			if(currentTileCoord != previousTileCoord)
			{
				Debug.Log(x + ", " + y + ": " + worldData.GetTileValue(x,y));
			}
			previousTileCoord = currentTileCoord;
			tileSelectionObject.transform.position = currentTileCoord;
		}
	}

	public Vector3 CurrentTileCoord
	{
		get{return currentTileCoord;}
	}


}
