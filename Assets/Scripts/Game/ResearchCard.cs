using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResearchCard : MonoBehaviour {
	public Researcher researcher;

	public void Notify()
	{
		foreach (Transform child in this.transform) {
			if(child.name == "Portrait")
			{
				Image portrait = child.GetComponent<Image>() as Image;
				portrait.sprite = researcher.portrait;	
			}
			if(child.name == "Name")
			{
				var name = child.GetComponent<Text>() as Text;
				name.text = researcher.name;
				this.transform.name = researcher.name + " card";
			}
			if(child.name == "Age")
			{
				var age = child.GetComponent<Text>() as Text;
				age.text = "Age " + researcher.age.ToString();
			}
			if(child.name == "Knowledge")
			{
				var knowledge = child.GetComponent<Text>() as Text;
				knowledge.text = "Knowledge " + researcher.researchKnowledge.Count.ToString();
				
			}
			if(child.name == "Skillset")
			{
				var skill = child.GetComponent<Text>() as Text;
				skill.text = "Skillset: n/a";
			}
		}
	}
}
