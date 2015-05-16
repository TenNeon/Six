using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;

public class SelectionBox : MonoBehaviour {
	public static SelectionBox main;
	
	Rect dimensions;
	bool selectionStarted = false;
	bool isSelecting = false;
	bool addToSelection = false;
	static bool suppressed = false; //when the box is suppressed it doesn't draw or update
	public Texture2D selectionBoxTexture;
	public Material boxMaterial;

	public List<GameObject> selectedObjects;

	bool selectionCheckedThisFrame = false;

	VectorLine box;

	// Use this for initialization
	void Start () {
		if (SelectionBox.main == null)
		{
			main = this;
		}

		InitBox();

		dimensions = new Rect();
		dimensions.xMin = dimensions.xMax = -1;
		selectedObjects = new List<GameObject>();


	}

	void InitBox()
	{
		Vector2[] points = new Vector2[5];
		points[0] = new Vector2(0,0);
		points[1] = new Vector2(0,0);
		points[2] = new Vector2(0,0);
		points[3] = new Vector2(0,0);
		points[4] = new Vector2(0,0);
		Color color = Color.red;

		box = new VectorLine("box"
		                     ,points
		                     ,color
		                     ,boxMaterial
		                     ,2f
		                     ,LineType.Continuous
		                     ,Joins.Weld);
		box.sortingOrder = 10;
	}

	void Update()
	{
		selectionCheckedThisFrame = false;
	}

	public static void Suppress ()
	{
		suppressed = true;
	}

	public static void Release()
	{
		suppressed = false;
	}

	void Select()
	{
		addToSelection = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
		if(!addToSelection){selectedObjects = new List<GameObject>();}

		GameObject foundObject = GetObjectUnderPointer();

		if(foundObject != null)
		{
			foreach (Transform child in foundObject.transform) {
				if (child.CompareTag("Selectable"))
				{
					if (selectedObjects.Contains(foundObject))
					{
						selectedObjects.Remove(foundObject);
					}
					else{
						selectedObjects.Add(foundObject);
					}
					NotifySelectionChanged();
					return;
				}
			}
		}
	}

	void NotifySelectionChanged()
	{
		Object[] objects = FindObjectsOfType (typeof(GameObject));
		foreach (GameObject go in objects) {
			go.SendMessage ("OnSelectionChanged", SendMessageOptions.DontRequireReceiver);
		}
	}
	
	GameObject GetObjectUnderPointer(){
		//8, 10, 11, 12
		int mask = 1<<8 | 1<<10 | 1<<11 | 1<<12;

		var hit = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition),mask);
		if(hit != null)
		{
			return hit.gameObject;
		}

		return null;	
	}

	void BoxSelect()
	{
		bool foundUnit = false;
		var foundObjects = GameObject.FindGameObjectsWithTag("Selectable");
		//if shift is held, don't clear
		addToSelection = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift));
		if(!addToSelection){selectedObjects.Clear();}

		for (int i = 0; i < foundObjects.Length; i++)
		{
			GameObject foundObject = foundObjects[i].transform.parent.gameObject;
			Vector3 convertedPoint3D = Camera.main.WorldToScreenPoint( foundObject.transform.position );
			Vector2 conertedPoint2D = new Vector2(convertedPoint3D.x, Camera.main.pixelHeight - convertedPoint3D.y);

			if(dimensions.Contains(conertedPoint2D,true))
			{
				if (selectedObjects.Contains(foundObject))
				{
					selectedObjects.Remove(foundObject);
				}
				else{
					selectedObjects.Add(foundObject);
				}
			}
		}

		FilterSelectedObjects();

		NotifySelectionChanged();
	}

	void FilterSelectedObjects ()
	{
		List<GameObject> unitOnlyList = new List<GameObject>();
		for (int i = 0; i < selectedObjects.Count; i++) {
			foreach (Transform child in selectedObjects[i].transform) {
				if (child.CompareTag("Unit"))
				{
					unitOnlyList.Add(selectedObjects[i]);
				}	
			}
		}
		if(unitOnlyList.Count > 0)
		{
			selectedObjects = unitOnlyList;
		}
	}
	
	void OnGUI()
	{		
		if( GUIUtility.hotControl != 0 || suppressed)
		{
			ResetSelectionBox();
			return;
		}

		if(Input.GetMouseButtonUp(0))
		{
			box.active = false;
			if (selectionCheckedThisFrame)
			{
				return;//Avoid checking selection multiple times per frame
			}
			selectionCheckedThisFrame = true;

			if(isSelecting)
			{
				BoxSelect();
				isSelecting = false;
			}	
			else{
				Select ();
			}
			ResetSelectionBox();
		}
		
		if(Input.GetMouseButton(0))
		{
			if(isSelecting)
			{
				dimensions.xMax = Input.mousePosition.x;
				dimensions.yMax = Camera.main.pixelHeight - Input.mousePosition.y;
			}
			else
			{
				//find out if it is now dragging
				Vector2 startPoint = new Vector2(dimensions.xMin, dimensions.yMin);
				Vector2 mousePoint = new Vector2(Input.mousePosition.x, Camera.main.pixelHeight - Input.mousePosition.y);
				if(startPoint != mousePoint && dimensions.xMin != -1)
				{
					isSelecting = true;
					box.active = true;
				}
			}
		}

		if(Input.GetMouseButtonDown(0))
		{
			//begin a selection box
			dimensions.xMin = Input.mousePosition.x;
			dimensions.yMin = Camera.main.pixelHeight - Input.mousePosition.y;
		}
		
		if(isSelecting)
		{
			if(!Input.GetMouseButton(0))
			{
				isSelecting = false;
				box.active = false;
			}
			else
			{
				DrawSelectionBox();
			}
		}
	}

	void ResetSelectionBox()
	{
		isSelecting = false;
		box.active = false;
		dimensions.xMin = -1;
		dimensions.yMin = -1;
	}

	void DrawSelectionBox()
	{
		box.active = true;
		box.MakeRect(new Vector3(dimensions.xMin,Camera.main.pixelHeight - dimensions.yMin,0), new Vector3(dimensions.xMax,Camera.main.pixelHeight - dimensions.yMax,0));
		box.Draw();
	}	
}
