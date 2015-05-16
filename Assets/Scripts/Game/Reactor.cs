using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Reactor : MonoBehaviour {
	public Recipe recipe;
	bool reactionPaused = false;
	float reactionSpeed = 1;
	bool reactionStarted = false;

	// Use this for initialization
	void Start () {
	
	}

	void Update () {
		if(reactionSpeed <= 0 || reactionPaused)
		{
			return;
		}

		if(!reactionStarted)
		{
			StartReaction();
		}

		if (! reactionPaused && reactionStarted && reactionSpeed > 0) 
		{
			React();
		}

	}

	bool InputsAvailable()
	{
		return true;
	}

	void StartReaction()
	{
		ConsumeInputs();
		reactionStarted = true;
	}

	void React()
	{
		//consume continuous inputs;
	}

	void ConsumeInputs()
	{
		//consume items from input list
	}

	void ProduceOutputs()
	{
		//produce items from output list
	}

	void AbortReaction()
	{
		//produce items from failure list
	}
}

public class Recipe
{
	List<ItemStack> inputs;
	List<ItemStack> continuousInputs;
	List<ItemStack> outputs;
	List<ItemStack> failureOutputs;
	enum OutputMode {Normal, RandomPick, RandomAmount};
	int numberOfRandomOutputs;
	float duration;
}
