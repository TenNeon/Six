using UnityEngine;
using System.Collections;

public class Cubescript : MonoBehaviour {
	float size = 1;
	float growthRate = 1f/60f;

	float energy = 1;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

		float energyUsedThisTurn = energy * growthRate * Time.deltaTime;
		energy -= energyUsedThisTurn;
		float sizeIncreaseThisTurn = energyUsedThisTurn;
		size += sizeIncreaseThisTurn;
		transform.localScale = new Vector3(1,1,size);
	}
}
