using UnityEngine;
using System.Collections;
using System.Xml;
public class RecipeKeeper : MonoBehaviour {
	string text;
	string fileLocation = @"E:\Programs\Unity\Unity Projects\Four-Dev\Assets\Data\ExampleRecipe.xml";
	void Start () {
		//load recipes from data files
		LoadFile();
	}

	void LoadFile()
	{
		text = System.IO.File.ReadAllText(fileLocation);

        //using (XmlReader reader = XmlReader.Create(new StringReader(xmlString)))
        //{

        //}
	}

}
