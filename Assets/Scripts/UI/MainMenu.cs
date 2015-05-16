using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
	public void StartNewGame()
	{
		Application.LoadLevel("MainGame");
	}

	public void LoadTileWorkspace()
	{
		Application.LoadLevel("GroundAndCameraDevelopment");
	}
}
