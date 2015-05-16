using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ResearchersPanel : MonoBehaviour {
	public GameObject researchSystemObj;
	ResearchSystem rs;
	//public GameObject panelObj;
	public GameObject cardPrefab;
	//Button startResearchButton;
	Text progressLabel;
	Text techLabel;

	public void Start()
	{
		Invoke("GeneratePanels", 0.01f);

		foreach (Transform child in this.transform) {
			if(child.name == "Tech")
			{
				techLabel = child.GetComponent<Text>() as Text;
			}
			if(child.name == "Progress")
			{
				progressLabel = child.GetComponent<Text>() as Text;
			}
		}

	}

	void Update()
	{
		techLabel.text = "Tech: " + rs.tech.ToString();
		if(rs.progress > 0)
		{
			string progress = (Mathf.RoundToInt( 100*rs.progress/rs.researchTaskSize )).ToString();
			progressLabel.text = "Progress: " + progress + "%";
		}
		else 
		{
			progressLabel.text ="";	
		}

	}

	public void AddResearcher()
	{
		rs = researchSystemObj.GetComponent<ResearchSystem>() as ResearchSystem;
		AddPanel( rs.AddNewResearcher(), rs.researchers.Count-1 );
	}

	public void GeneratePanels()
	{
		rs = researchSystemObj.GetComponent<ResearchSystem>() as ResearchSystem;
		int cardWidthPix = 150;
		int cardHeightPix = 250;
		Debug.Log ("Researcher panels: " + rs.researchers.Count);
		for (int i = 0; i < rs.researchers.Count; i++) {
			AddPanel (rs.researchers[i], i);
		}
	}

	void AddPanel(Researcher newResearcher, int index)
	{
		int cardWidthPix = 150;
		int cardHeightPix = 250;
		int currentResearchersCount = rs.researchers.Count;

		Debug.Log ("Researcher: " + newResearcher.name);

		var newCard = (Instantiate(cardPrefab) as GameObject).transform;
		var cardScript = newCard.GetComponent<ResearchCard>() as ResearchCard;
		cardScript.researcher = newResearcher;

		foreach (Transform child in newCard.transform) {
			if(child.name == "Portrait")
			{
				Image portrait = child.GetComponent<Image>() as Image;
				portrait.sprite = newResearcher.portrait;	
			}
			if(child.name == "Name")
			{
				var name = child.GetComponent<Text>() as Text;
				name.text = newResearcher.name;
				newCard.name = newResearcher.name + " card";
			}
			if(child.name == "Age")
			{
				var age = child.GetComponent<Text>() as Text;
				age.text = "Age " + newResearcher.age.ToString();
			}
			if(child.name == "Knowledge")
			{
				var knowledge = child.GetComponent<Text>() as Text;
				knowledge.text = "Knowledge " + newResearcher.researchKnowledge.Count.ToString();
				
			}
			if(child.name == "Skillset")
			{
				var skill = child.GetComponent<Text>() as Text;
				skill.text = "Skillset: n/a";
			}
		}
		newCard.SetParent(transform);
		
		var rectTransform = newCard.GetComponent<RectTransform>();

		//set position
		rectTransform.offsetMin = new Vector2(5+cardWidthPix*index,-cardHeightPix);
		rectTransform.offsetMax = new Vector2(cardWidthPix+cardWidthPix*index,-5);
		rectTransform.anchorMin = new Vector2(0,1);
		rectTransform.anchorMax = new Vector2(0,1);	
	}

	public void StartResearch()
	{
		rs.StartResearch();

//		if (rs.isResearching) 
//		{
//			startResearchButton.enabled = false;
//		}

		foreach (Transform child in this.transform) {
			if (System.Text.RegularExpressions.Regex.IsMatch(child.name, ".* card"))
			{
				//update all the fields to match the model
				var card = child.GetComponent<ResearchCard>() as ResearchCard;
				card.Notify();
			}
		}
		foreach (Transform child in this.transform) {
			if(child.name == "Tech")
			{
				var tech = child.GetComponent<Text>() as Text;
				tech.text = "Tech: " + rs.tech.ToString();
			}
		}
		//UpdatePanels();
	}
}
