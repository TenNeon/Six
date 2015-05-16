using UnityEngine;
using System.Collections;

public class WorldData : MonoBehaviour {
	public int width = 1000;
	public int height = 1000;
	public int tileSize = 16;
	int[,] tiles;
	public Texture2D terrainTiles;
	// Use this for initialization
	void Start () {
		InitTiles();
		BuildTexture();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void InitTiles ()
	{
		tiles = new int[width,height];
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				tiles[i,j] += Random.Range(0,8);
				tiles[i,j] += Random.Range(0,8);
				tiles[i,j] += Random.Range(0,8);
				tiles[i,j] += Random.Range(0,8);
			}
		}
	}


	public int GetTileValue(int x, int y)
	{
		return tiles[x,y];
	}

	Color[][] PrepareTiles()
	{
		int tilesPerRow = Mathf.FloorToInt( terrainTiles.width/tileSize );
		int rows = Mathf.FloorToInt( terrainTiles.height/tileSize );
		Color[][] preparedTiles = new Color[tilesPerRow*rows][];

		for (int y = 0; y < rows; y++) {
			for (int x = 0; x < tilesPerRow; x++) {
				preparedTiles[y*tilesPerRow + x] = terrainTiles.GetPixels(x*tileSize, y*tileSize, tileSize, tileSize);
			}
		}

		return preparedTiles;
	}

	void BuildTexture ()
	{
		int tilesPerRow = Mathf.FloorToInt( terrainTiles.width/tileSize );
		int rows = Mathf.FloorToInt( terrainTiles.height/tileSize );
		width = 54;
		height = 27;
		int texWidth = tileSize * width;
		int texHeight = tileSize * height;
		Color[][] preparedTiles = PrepareTiles();

		//Texture2D texture = new Texture2D(width*tileSize,height*tileSize);
		Texture2D texture = new Texture2D(texWidth,texHeight);

		Color c = new Color(Random.Range (0f,1f), Random.Range(0f,1f), Random.Range(0f,1f) );
		for (int y = 0; y < height; y++) {
			for (int x = 0; x < width; x++) {
				int num = Random.Range (0,4);

				Color[] p = preparedTiles[num];
				texture.SetPixels(x*tileSize,y*tileSize, tileSize,tileSize, p);
			}
		}
		texture.filterMode = FilterMode.Point;
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.Apply();

		MeshRenderer meshRenderer = GetComponent<MeshRenderer>() as MeshRenderer;
		meshRenderer.sharedMaterial.mainTexture = texture;
	}
}

public class Plant
{
	public float size = 1;
}
