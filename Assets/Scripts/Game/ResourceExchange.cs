using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Vectrosity;

public class ResourceExchange : MonoBehaviour {

	List<Order> orders = new List<Order>();

	public static List<ResourceExchange> exchanges;

	bool showList = false;
	bool showLines = false;
	public Material lineMaterial;
	public Texture2D frontTex;
	public Texture2D backTex;

	VectorLine lines;

	public void Start()
	{
		InitLines ();
		if(exchanges == null)
		{
			exchanges = new List<ResourceExchange>();
		}
		exchanges.Add(this);
	}

	void InitLines ()
	{
		int numberOfLines = 50;
		Vector3[] points = new Vector3[2 * numberOfLines];
		points [0] = Vector3.zero;
		points [1] = Vector3.zero;
		lines = new VectorLine ("lines", points, lineMaterial, 1, LineType.Discrete);
		lines.lineWidth = 5f;
		lines.sortingOrder = 0;
		lines.sortingLayerID = 3;
	}

	public void PlaceOrder(Order o)
	{
		o.exchange = this;
		orders.Add(o);
	}

	//returns the oldest order of orderType that is equal to or less than amount
	public List<Order> FindOrders(Order.OrderType orderType, string resourceType, int amountToFind = 1)
	{
		List<Order> matchingOrders = (from x in orders
		                              where x.OrderTaker == null 
		                                && x.orderType == orderType 
		                              	&& x.resourceType == resourceType 
		                              	&& x.amount <= amountToFind
		                              select x).OrderBy(y=>y.orderPlacedTime).ToList();
		List<Order> ordersToReturn = new List<Order>();
		int amountFound = 0;
		for(int i = 0; i < matchingOrders.Count && amountFound < amountToFind; i++)
		{
			if(amountFound + matchingOrders[i].amount <= amountToFind)
			{
				amountFound += matchingOrders[i].amount;
				ordersToReturn.Add(matchingOrders[i]);
			}
		}
		return matchingOrders;
	}

	public bool TakeOrder(Order o)
	{
		return orders.Remove(o);
	}

	public void Complete(Order o)
	{
		orders.Remove(o);
	}

	public void RemoveOrder(Order o)
	{
		orders.Remove(o);
	}

	public Vector2 scrollPosition;
	void OnGUI()
	{
		if(showList)
		{
			DrawList();
		}
		if(showLines)
		{
			DrawLines();
		}
		string buttonText = "Show Orders";
		if(showList){buttonText = "Hide Orders";}
		if (GUI.Button(new Rect(Screen.width - 130, 10, 120, 25), buttonText))
		{
			showList = !showList;
		}
		buttonText = "Show Order Lines";
		if(showLines){buttonText = "Hide Order Lines";}
		if (GUI.Button(new Rect(Screen.width - 130, 40, 120, 25), buttonText))
		{
			showLines = !showLines;
			lines.active = showLines;
		}
	}

	void DrawList()
	{
		Color oldColor = GUI.contentColor;
		GUI.contentColor = Color.black;
		string currentItemTime;
		scrollPosition = GUILayout.BeginScrollView(scrollPosition);
		foreach (var item in orders) 
		{
			if(item.OrderTaker != null)
				GUI.contentColor = Color.yellow;

			if(item.OrderPlacer != null && item.assignedWorkers.Count >= item.amount)
			{
				GUI.contentColor = Color.green;
			}

			if(item.OrderPlacer == null)
			{
				if(item.OrderTaker == null)
				{
					GUI.contentColor = Color.blue;
				}
				else
				{
					GUI.contentColor = Color.green;
				}
			}
			currentItemTime = (Time.time - item.orderPlacedTime).ToString("F1");

			string placerName;
			if(item.OrderPlacer != null)
			{
				placerName = item.OrderPlacer.owner.name;
			}
			else if (item.assignedWorkers.Count > 0)
			{
				placerName = "Worker";
			}
			else
			{
				placerName = "Unknown";
			}
			GUILayout.Label("#"+item.OrderNumber+" ("+currentItemTime +") "+ placerName + ": " 
			                + item.orderType.ToString() + " [" 
			                + item.resourceType 
			                + "] Workers: " + item.assignedWorkers.Count + "/" +item.amount 
			                );
			GUI.contentColor = Color.black;
		}
		GUILayout.EndScrollView();
		GUI.contentColor = oldColor;
	}

	void DrawLines()
	{
		Vector3 start;
		Vector3 end;
		Color[] colors = new Color[50];
		Color color;
		Color unfilledOrderColor = new Color(0.5f,0.5f,0.5f,0.5f);
		Color filledOrderColor = new Color(0.9f,0.9f,0f,0.5f);
		Color activeOrderColor = new Color(0,0.9f,0,0.5f);
		for(int i = 0; i < orders.Count; i++)
		{
			color = unfilledOrderColor;
			if(orders[i].OrderTaker != null)
			{
				start = orders[i].GetOrigin().transform.position;
				end = orders[i].GetDestination().transform.position;
			}
			else if (orders[i].GetOrigin() != null){
				ResourceProfile origin = orders[i].GetOrigin();
				start = origin.transform.position;
				Vector3 diff = origin.transform.position - origin.owner.transform.position;
				end = start + diff.normalized*0.8f;
			}
			else
			{
				ResourceProfile destination = orders[i].GetDestination();
				end = destination.transform.position;
				Vector3 diff = destination.transform.position - destination.owner.transform.position;
				start = end + diff.normalized*0.8f;
			}
			if(orders[i].OrderTaker != null)
			{
				color = activeOrderColor;
				if(orders[i].assignedWorkers.Count >= orders[i].amount)
				{
					color = filledOrderColor;
				}
			}
			if(i*2+1 < lines.points3.Length)
			{
				lines.points3[i*2] = start;
				lines.points3[i*2+1] = end;
				colors[i] = color;
			}

			Debug.DrawLine(start,end,color,0.5f,false);
		}
		lines.SetColors(colors);
		lines.Draw();
	}	
}