using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Resource : MonoBehaviour {
	//assemble a dictionary of resource names, and their sprites
	public List<Sprite> sprites = new List<Sprite>();
	public List<string> names = new List<string>();
	public Dictionary<string,Sprite> resources = new Dictionary<string,Sprite>();
	static bool dictionaryInitialized = false;

	static Resource main;
	
	public void Start()
	{
		if (main == null)
		{
			main = this;
			CreateDictionary();
		}
	}
	
	
	static void CreateDictionary()
	{
		if(main.sprites.Count != main.names.Count)
		{
			Debug.LogError("Resource sprite count does not match name count.");
		}
		else
		{
			for(int i = 0; i < main.sprites.Count; i++)
			{
				main.resources.Add(main.names[i],main.sprites[i]);
			}
			dictionaryInitialized = true;
		}
	}
	
	public static Sprite GetResourceSprite(string resourceType)
	{
		if(!dictionaryInitialized)
		{
			CreateDictionary();
		}

		if(main.resources.ContainsKey(resourceType))
		{
			return main.resources[resourceType];
		}


		return null;
	}
}
