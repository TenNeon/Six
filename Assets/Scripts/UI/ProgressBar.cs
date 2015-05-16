using UnityEngine;
using System.Collections;

public class ProgressBar : MonoBehaviour {
	public static ProgressBar main;

	void Start()
	{
		main = this;
	}

	public Texture2D texture;
	public Texture2D backgroundTexture;
	public Texture2D foregroundTexture;
}
