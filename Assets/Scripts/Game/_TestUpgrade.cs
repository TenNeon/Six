using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _TestUpgrade{
	public string upgradeName ="unnamed upgrade";
	public List<_TestUpgrade> prerequisites = new List<_TestUpgrade>();
	public Dictionary<string,float> requirments = new Dictionary<string, float>();
	public Dictionary<string,float> effects = new Dictionary<string, float>();
	public bool applied = false;

	public bool CanApply(IUpgradable target)
	{
	
		return CheckPrerequisites(target) && CheckRequirements(target);
	}

	public bool Apply(IUpgradable target)
	{
		//check for prerequisites
		if( !CheckPrerequisites (target) ) 
		{
			Debug.Log("Prerequisites not met.");
			return false;
		}
		//check for requirements
		if( !CheckRequirements (target) ) {return false;}
		//if we get here, all requirements are met
		//apply effects.
		ApplyEffects(target);
		applied = true;
		return true;
	}

	// Applies all stat-changing effects in the effects list. 
	// Ignores changes in cases where the target does not have the named stat.
	void ApplyEffects(IUpgradable target)
	{
		var stats = target.GetStats();

		foreach (var effect in effects) 
		{
			if (stats.ContainsKey(effect.Key)) {
				stats[effect.Key] += effect.Value;
				Debug.Log("Applied effect: " + effect.Key + " " + effect.Value);
			}
			else {
				Debug.Log("Upgrade failed to find stat for change: " + effect.Key);
			}
		}
		target.SetStats(stats);
	}

	bool CheckPrerequisites(IUpgradable target)
	{
		var upgrades = target.GetUpgrades();

		for(int i = 0; i < prerequisites.Count; i++)
		{
			var upg = upgrades.Find(x => x.upgradeName == prerequisites[i].upgradeName);
			if(upg == null || upg.applied == false)
			{
				return false;
			}
		}
		return true;
	}

	bool CheckRequirements (IUpgradable target)
	{
		var stats = target.GetStats();
		foreach (var req in requirments) {
			float valToMeet = req.Value;
			float val = 0;
			if (stats.ContainsKey (req.Key)) {
				val = stats [req.Key];
			}
			else {
				return false;
			}
			if (val < valToMeet) {
				return false;
			}
		}
		return true;
	}
}

public interface IUpgradable
{
	Dictionary<string, float> GetStats();
	void SetStats(Dictionary<string, float> newStats);
	List<_TestUpgrade> GetUpgrades();
}
