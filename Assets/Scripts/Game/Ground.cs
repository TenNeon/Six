using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Ground : MonoBehaviour {
	public static Ground ground;

	public Texture2D groundTex;
	public Cell[,] groundArray;

	public bool resetOnLoad = true;
	int w;
	int h;

	public int W {
			get
			{
				return w;
			}
	}

	public int H {
		get
		{
			return h;
		}
	}

	// Use this for initialization
	void Start () {

		//InvokeRepeating ("Resize",0,.2f);

		var ground = GetComponent("Ground") as Ground;
		Ground.ground = ground;

		var guiTexture = GetComponent("GUITexture") as GUITexture;

		guiTexture.pixelInset = Camera.main.pixelRect;

		groundTex = (GetComponent("GUITexture") as GUITexture).texture as Texture2D;

		w = groundTex.width;
		h = groundTex.height;

		InitCells();
		if(resetOnLoad)
		{
			InitImage();
		}

	}

	void OnWindowResize () {
		//var guiTexture = GetComponent("GUITexture") as GUITexture;
		//guiTexture.pixelInset = Camera.main.pixelRect;
	}

	void InitCells()
	{
		groundArray = new Cell[w,h];
		for (int i = 0; i < w; i++)
		{
			for(int j = 0; j < h; j++)
			{
				Cell c = new Cell();
				groundArray[i,j] = c;
				c.AddItem("Ore", 100);
			}
		}
	}

	//Sets ore layer based on the colot of groundtex
	void InitImage()
	{
		Color[] pix = groundTex.GetPixels(0, 0, w, h);
		Color c = new Color();

		for(int i = 0; i < h-1; i++)
		{
			for(int j = 0; j-1 < w; j++)
			{
				Debug.Log (i + " " + j + " | " + w + " " + h);
				var targetCell = groundArray[i,j];
				c = Color.Lerp(Color.black,Color.white, targetCell.GetAmount("Ore")/100f);
				if(i == 1 && j == 1){Debug.Log(c.r + " " + c.g + " " +c.b);}
				pix[i*w + j] = c;
			}
		}
		
		groundTex.SetPixels(pix);
		groundTex.Apply();
	}

	public int AddMaterial(string materialType, int amount, Vector2 position, Vector2 offset = new Vector2())
	{
		int amountChanged = 0;
		position = Camera.main.WorldToScreenPoint(position);
		var cameraW = Camera.main.pixelWidth;
		var cameraH = Camera.main.pixelHeight;
		int x = Mathf.FloorToInt(position.x*(w/cameraW)) + Mathf.RoundToInt(offset.x);
		int y = Mathf.FloorToInt(position.y*(h/cameraH)) + Mathf.RoundToInt(offset.y);

		if (x >= w ) {x = w-1;}
		if (x <= 0 ) {x = 0;}
		if (y >= h ) {y = h-1;}
		if (y <= 0 ) {y = 0;}

		Cell targetCell = groundArray[x,y];
		if(targetCell != null)
		{
			amountChanged = targetCell.AddItem(materialType,amount);
		}
			
		Color[] pix = groundTex.GetPixels(0, 0, w, h);
		Color c = Color.Lerp(Color.black,Color.white, targetCell.GetAmount(materialType)/100f);
		pix[y*w + x] = c;

		groundTex.SetPixels(pix);
		groundTex.Apply();
		return amountChanged;
	}
}
