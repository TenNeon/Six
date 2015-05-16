using UnityEngine;
using System.Collections.Generic;

public class ItemStack{
	private int _count = 0;
	private string _itemType;

	public ItemStack(string type, int count = 0)
	{
		_itemType = type;
		_count = count;
	}

	public int Count {
			get { return _count; }
			set { 
				_count = value; 
			}
	}

	public string ItemType {
		get { return _itemType; }
	}

}
