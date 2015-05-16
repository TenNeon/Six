using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class WorkerFinder : MonoBehaviour {
	public static List<WorkerFinder> workerFinders;
	float searchDistance = 15f; //look this far from start point for workers
	public enum WorkerFinderOutputMessage {FullWorkerFound, NoWorkerFound, PartialWorkerFound, MissingEndpoint, BadOrderType};

	void Start()
	{
		if(workerFinders == null)
		{
			workerFinders = new List<WorkerFinder>();
		}

		workerFinders.Add(this);
	}

	public WorkerFinderOutputMessage AttachWorkersToOrder(Order o)
	{
		//find workers near the start point of the delivery
		ResourceProfile startPoint;
		ResourceProfile endPoint;

		if(o.orderType == Order.OrderType.WTB)
		{
			startPoint = o.OrderTaker;
			endPoint = o.OrderPlacer;
		}
		else if(o.orderType == Order.OrderType.WTS)
		{
			startPoint = o.OrderPlacer;
			endPoint = o.OrderTaker;
		}
		else 
		{
			return WorkerFinderOutputMessage.BadOrderType;
		}

		if (startPoint == null || endPoint == null) 
		{
			return WorkerFinderOutputMessage.MissingEndpoint;
		}

		// for each unit of task size
		// find and assign an onoccupied worker in range

		List<Worker> foundWorkers = (from x in Worker.workers
		                             where x.state == Worker.State.Idle 
		                             && Vector3.Distance(x.transform.position,startPoint.transform.position) <= searchDistance
									 select x
									).OrderBy(y => Vector3.Distance(y.transform.position,startPoint.transform.position)).ToList();

		// Assumes 1 worker can move 1 amount. This will have to be changed if higher-capacity workers are added
		if(foundWorkers.Count == 0)
		{
			return WorkerFinderOutputMessage.NoWorkerFound;
		}

		// I found x workers
		// I need y workers
		// assign from x to y until either x is empty or y is full
		for (int i = 0; i < o.amount; i++) 
		{
			if(i < foundWorkers.Count)
			{
				o.assignedWorkers.Add(foundWorkers[i]);
				foundWorkers[i].AssignOrder(o);
			}
			else 
			{
				return WorkerFinderOutputMessage.PartialWorkerFound;	
			}
		}
		return WorkerFinderOutputMessage.FullWorkerFound;
	}
}
