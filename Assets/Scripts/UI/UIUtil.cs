using UnityEngine;
using System.Collections;

public class UIUtil : MonoBehaviour {

	public static GameObject GetObjectUnderPointer(){
		//8, 10, 11, 12, 15
		int mask = 1<<8 | 1<<10 | 1<<11 | 1<<12 | 1<<15;
		
		var hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition),mask);
		if(hit != null)
		{
			return hit.gameObject;
		}
		
		return null;	
	}
}
