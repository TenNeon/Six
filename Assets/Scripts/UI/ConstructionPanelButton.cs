using UnityEngine;
using System.Collections;

public class ConstructionPanelButton : MonoBehaviour {
	public ConstructionPanel owner;
	public GameObject buildingPrefab;

	void Start()
	{
		owner = transform.parent.GetComponent<ConstructionPanel>() as ConstructionPanel;
	}

	public void ButtonClicked()
	{
		owner.ChildButtonClicked(buildingPrefab);
	}

}
