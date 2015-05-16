using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cell {
	List<ItemStack> items = new List<ItemStack>();

	public int AddItem(string itemType, int amount)
	{
		//find the item
		var found = items.Find(x => x.ItemType.Equals( itemType ));
		int amountDifference = 0;


		if(found != null)
		{
			//add amount but not below 0
			if(found.Count + amount > 0)
			{
				amountDifference = -amount;
				found.Count += amount;
			}
			else
			{
				amountDifference = found.Count;
				items.Remove(found);
			}
		}
		else if(amount > 0){
			ItemStack newStack = new ItemStack(itemType, amount);
			items.Add(newStack);
			amountDifference = -amount;
		}
		return amountDifference;

	}

	public int GetAmount(string itemType)
	{
		var found = items.Find(x => x.ItemType.Equals( itemType ));
		if(found != null)
		{
			return found.Count;
		}
		else {
			return 0;	
		}
	}
	
}
