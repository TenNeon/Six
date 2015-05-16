using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Order
{
	static int nextOrder = 1;
	ResourceProfile orderPlacer;
	ResourceProfile orderTaker;
	public List<Worker> assignedWorkers = new List<Worker>();
	public enum OrderType {WTS, WTB};
	public OrderType orderType;
	public int amount;
	public string resourceType;
	public float orderPlacedTime;
	public ResourceExchange exchange;
	int orderNumber;
	public int OrderNumber {
		get{return orderNumber;}
	}
	
	public ResourceProfile OrderPlacer {
		get{return orderPlacer;}
		set
		{
			if(orderPlacer == null){orderPlacer = value;}
			else {
				Debug.LogError("Attempted to change order placer for order #"+orderNumber);
			}
		}
	}
	public ResourceProfile OrderTaker {
		get{return orderTaker;}
		set
		{
			if(orderTaker == null){orderTaker = value;}
			else {
				Debug.LogError("Attempted to change order taker for order #"+orderNumber);
			}
		}
	}

	public Order ()
	{
		orderNumber = Order.nextOrder;
		Order.nextOrder += 1;
	}
	//the origin is the placer if it is wts
	//the origin is the taker if it is wtb
	public ResourceProfile GetOrigin()
	{
		if (orderType == OrderType.WTS)
		{
			return orderPlacer;
		}
		else if (orderType == OrderType.WTB)
		{
			return orderTaker;	
		}
		Debug.LogError("Bad order type.");
		return null;
	}
	
	public ResourceProfile GetDestination()
	{
		if (orderType == OrderType.WTS)
		{
			return orderTaker;
		}
		else if (orderType == OrderType.WTB)
		{
			return orderPlacer;	
		}
		Debug.LogError("Bad order type.");
		return null;
	}
	
	public void Complete(int amountCompleted)
	{
		amount -= amountCompleted;
		if (amount <= 0)
		{
			orderPlacer.CompleteOrder(this);
			orderTaker.CompleteOrder(this);
		}
	}

	public void Cancel(bool cargoLost)
	{
		exchange.RemoveOrder (this);

		//notify workers
		for (int i = 0; i < assignedWorkers.Count; i++) {
			assignedWorkers[i].OrderCanceled();	
		}

		//notify placer
		if(orderPlacer != null)
		{
			orderPlacer.OrderCanceled(this,cargoLost);
		}

		//notify taker
		if(orderTaker != null)
		{
			orderTaker.OrderCanceled(this,cargoLost);
		}
	}

	public void AssignWorker(Worker w)
	{
		if(!assignedWorkers.Contains(w))
		{
			assignedWorkers.Add(w);
		}
	}
}

