using UnityEngine;
using System.Collections;
using Vectrosity;

public class SelectionIndicator : MonoBehaviour {
	public Texture2D selectionIndicatorTexture;
	public static SelectionIndicator main;
	int maxSelection = 100;

	void Start()
	{
		if(main == null)
		{
			main = this;
		}
	}
	
	void OnGUI()
	{
		foreach (var item in SelectionBox.main.selectedObjects) {
			var pos = Camera.main.WorldToScreenPoint(item.transform.position);
			Rect rect = new Rect(pos.x-10,Screen.height-pos.y-10,20,20);
			GUI.DrawTexture(rect,selectionIndicatorTexture);
		}
	}

//	void OnGUI()
//	{
//		if(Event.current.type == EventType.Repaint)
//		{
//			for(int i = 0; i < SelectionBox.main.selectedObjects.Count; i++)
//			{ 
//				Debug.Log (i);
//				var pos = Camera.main.WorldToScreenPoint( SelectionBox.main.selectedObjects[i].transform.position );
//				var r =  SelectionBox.main.selectedObjects[i].GetComponent("Renderer") as Renderer;
//				if(r != null)
//				{
//					var extents = r.bounds.extents*100;
//					var xBound = extents.x;
//					var yBound = extents.y;
//					var rect = new Rect(pos.x-xBound,pos.y+yBound,xBound*2,yBound*2);
//					DrawBoundingBox(rect);
//				}
//				else{Debug.LogError("No Renderer found for bounding box draw.");}
//			}
//		}
//		//box.active = false;
//	}




//	void OnGUI()
//	{
//		if(Event.current.type == EventType.Repaint && SelectionBox.main.selectedObjects.Count > 0)
//		{
//			//calculate how many lines we need
//			int numberOfPoints = SelectionBox.main.selectedObjects.Count * 8;
//
//			//create the array to hold the points
//			Vector2[] points = box.points2;
//			for(int i = 0; i < numberOfPoints/8; i+=8)
//			{ 
//				var pos = Camera.main.WorldToScreenPoint( SelectionBox.main.selectedObjects[i].transform.position );
//				var r =  SelectionBox.main.selectedObjects[i].GetComponent("Renderer") as Renderer;
//
//				var extents = r.bounds.extents*100;
//				var xBound = extents.x;
//				var yBound = extents.y; 
//
//				points[i  ] = new Vector2(pos.x-xBound,pos.y+yBound);//top-left
//				points[i+1] = new Vector2(pos.x+xBound,pos.y+yBound);//top-right
//
//				points[i+2] = new Vector2(pos.x+xBound,pos.y+yBound);//top-right
//				points[i+3] = new Vector2(pos.x+xBound,pos.y-yBound);//bottom-right
//
//				points[i+4] = new Vector2(pos.x+xBound,pos.y-yBound);//bottom-right
//				points[i+5] = new Vector2(pos.x-xBound,pos.y+yBound);//bottom-left
//
//				points[i+6] = new Vector2(pos.x-xBound,pos.y+yBound);//bottom-left
//				points[i+7] = new Vector2(pos.x-xBound,pos.y+yBound);//top-left
//			}
//			for(int i = numberOfPoints; i < points.Length; i++)
//			{
//				points[i] = new Vector2(0,0);
//			}
//
//			if(box != null){
//				box.active = true;
//				box.points2 = points;
//				box.Draw ();
//			}
//		}
//		if(SelectionBox.main.selectedObjects.Count == 0)
//		{
//			if(box != null){
//				box.active = false;
//			}
//		}
//	}

//	var linePoints = new Vector2[numberOfPoints];
//	
//	for (p in linePoints)
//		p = Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
//	
//	var lineColors = new Color[numberOfPoints/2];
//	
//	for (c in lineColors)
//		c = Color(Random.value, Random.value, Random.value);
//	
//	var line = new VectorLine("Line", linePoints, lineColors, null, lineWidth);
//	
//	line.Draw();
}
