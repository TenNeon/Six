using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpawnerUI : MonoBehaviour {
	//ResourceSpawn controller;
	Text Name;
	Text OutputText;
	Text SpawnRateText;
	Text SpawnAmountText;
	Text SpawnRadiusText;
	Slider SpawnRateSlider;
	Slider SpawnAmountSlider;
	Slider SpawnRadiusSlider;
	Button OutputButton;
	Image itemsListPanel;
	bool showPanel = false;
	ItemPanelScript itemPanelScript;

	// Use this for initialization
	void Start () {
		//controller = this.transform.GetComponent<ResourceSpawn>() as ResourceSpawn;
		Transform canvas = null;
		foreach (Transform child in this.transform) {
			if(child.name == "Canvas")
			{
				canvas = child;
			}
		}
		if(canvas == null)
		{
			Debug.Log("Spawner UI: No canvas found");
			return;
		}
        //if(controller == null)
        //{
        //    Debug.Log("Spawner UI: No controller found");
        //    return;
        //}

		foreach (Transform child in canvas.transform) {
			if(child.name == "Name")
			{
				Name = child.GetComponent<Text>() as Text;
			}
			if(child.name == "Output")
			{
				OutputText = child.GetComponent<Text>() as Text;
			}
			if(child.name == "Spawn rate")
			{
				SpawnRateText = child.GetComponent<Text>() as Text;
			}
			if(child.name == "Spawn amount")
			{
				SpawnAmountText = child.GetComponent<Text>() as Text;
			}
			if(child.name == "Spawn radius")
			{
				SpawnRadiusText = child.GetComponent<Text>() as Text;
			}
			if(child.name == "Spawn Rate Slider")
			{
				SpawnRateSlider = child.GetComponent<Slider>() as Slider;
			}
			if(child.name == "Spawn Amount Slider")
			{
				SpawnAmountSlider = child.GetComponent<Slider>() as Slider;
			}
			if(child.name == "Spawn Radius Slider")
			{
				SpawnRadiusSlider = child.GetComponent<Slider>() as Slider;
			}
			if(child.name == "Output Button")
			{
				OutputButton = child.GetComponent<Button>() as Button;
			}
			if(child.name == "Items List Panel")
			{
				itemsListPanel = child.GetComponent<Image>() as Image;
				itemPanelScript = itemsListPanel.GetComponent<ItemPanelScript>() as ItemPanelScript;
			}
		}
        //Name.text = controller.Name;
        //OutputText.text = "Produces: " + controller.outputPrefab.name;
        //SpawnRateText.text = "Spawn rate: " + controller.SpawnsPerSecond;
        //SpawnAmountText.text = "Amount per spawn: " + controller.SpawnAmount;
        //SpawnRadiusText.text = "Spawn radius: " + controller.SpawnZoneRadius;
		itemPanelScript.UpdateItemsPanel();
	}

	public void SetSpawnAmount()
	{
		//controller.SpawnAmount = Mathf.RoundToInt( SpawnAmountSlider.value );
		//SpawnAmountText.text = "Amount per spawn: " + controller.SpawnAmount;
	}

	public void SetSpawnRate()
	{
		//controller.SpawnsPerSecond = SpawnRateSlider.value;
		//SpawnRateText.text = "Spawn rate: " + controller.SpawnsPerSecond.ToString("F1");
	}

	public void SetSpawnRadius()
	{
		//controller.SpawnZoneRadius = SpawnRadiusSlider.value;
		//SpawnRadiusText.text = "Spawn radius: " + controller.SpawnZoneRadius.ToString("F1");
	}

	public void SetOutput(Transform newOutput)
	{
		//controller.outputPrefab = newOutput;
		//OutputText.text = "Produces: " + controller.outputPrefab.name;
	}
	
	public void OutputButtonClicked()
	{
		itemPanelScript.UpdateItemsPanel();
		showPanel = !showPanel;
		itemsListPanel.gameObject.SetActive(showPanel);
	}


}
