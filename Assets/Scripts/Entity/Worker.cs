using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public interface IWorkerOwner
{
	bool AssignWorker(Worker w);
	void UnassignWorker(Worker w);
	void AssignWorkersToOrder(Order o);
	Vector3 IdlePosition();
}

public class Worker : MonoBehaviour {
	public static List<Worker> workers;
	static int workerNumber = 1;

	Transform currentTarget;
	Vector3 MoveTarget;
	float speed = .7f;
	float collectionDistance = 0.15f;
	public string cargo = "no cargo";
	int cargoCount = 0;
	int cargoCapacity = 1;
	public enum State {Assigned, PickingUpCargo, Loading, Transporting, Unloading, Idle, IdleLoaded, Stopped, Moving, Error };
	// Idle-> Assigned -> PickingUpCargo -> Loading -> Transporting -> Unloading -> Idle
	public State state = State.Idle;
	Order currentOrder;
	SpriteRenderer cargoSpriteRenderer;
	public IWorkerOwner owner;
	bool autonomous = true;

	public static int GetNextWorker()
	{
		return workerNumber++;
	}

	// Use this for initialization
	void Start () 
	{
		if(workers == null)
		{
			workers = new List<Worker>();
		}

		this.name = "Worker #" + GetNextWorker().ToString();

		workers.Add(this);
		foreach (Transform child in transform)
		{
			if(child.name == "Cargo")
			{
				cargoSpriteRenderer = child.GetComponent("SpriteRenderer") as SpriteRenderer;
				break;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		switch (state) {
			case (State.Assigned):
				currentTarget = currentOrder.GetOrigin().transform;
				EnterState(State.PickingUpCargo);
				break;
			case (State.PickingUpCargo):
				MoveTowardTarget();
				if(AtTarget()){EnterState(State.Loading);}
				break;
			case (State.Loading):
				Load();
				currentTarget = currentOrder.GetDestination().transform;
				EnterState(State.Transporting);
				break;
			case (State.Transporting):
				MoveTowardTarget();
				if(AtTarget())
				{EnterState(State.Unloading);}
				break;
			case (State.Unloading):
				Unload();
				currentTarget = null;
				EnterState(State.Idle);
				break;
			case (State.Idle):
				if (cargoCount > 0) {
					EnterState(State.IdleLoaded);
					SellCargo();
				}
				break;
			case (State.IdleLoaded):	
				if(currentOrder == null)
				{
					SellCargo();
				}
				else if (currentOrder.OrderTaker != null)
				{
					currentTarget = currentOrder.OrderTaker.transform;
					EnterState(State.Transporting);
				}
				break;
			case (State.Stopped):
				break;
			case (State.Moving):
				MoveTowardTarget();
				if(AtTarget()){
					if(autonomous)
					{
						EnterState(State.Idle);
					}
					else
					{
						EnterState(State.Stopped);
					}
				}
				break;
		default:
				break;
		}
	}

	void EnterState(State newState)
	{
		if(newState == State.Idle && Vector3.Distance(transform.position, owner.IdlePosition()) > 1.5f)
		{
			MoveTarget = owner.IdlePosition();

			newState = State.Moving;
		}


		if (newState == State.Idle || newState == State.Stopped || newState == State.IdleLoaded || newState == State.Error)
		{
			gameObject.layer = 11;
		}

		if(newState == State.Moving || newState == State.Transporting || newState == State.PickingUpCargo)
		{
			gameObject.layer = 16;
		}

		state = newState;
	}

	bool IsSelected()
	{
		if(SelectionBox.main != null)
		{
			if(SelectionBox.main.selectedObjects.Contains(this.gameObject))
			{
				return true;
			}
		}
		else
		{
			Debug.LogError("No SelectionBox found!");
		}
		return false;
	}

	void Load()
	{
		ResourceProfile targetProfile = currentTarget.GetComponent("ResourceProfile") as ResourceProfile;
		int returnedCargo = targetProfile.Claim(cargoCapacity);
		cargoCount += returnedCargo;
		cargo = currentOrder.resourceType;
		SetCargoSprite();
		Sprite cargoSprite = Resource.GetResourceSprite(cargo);
//		var cargoRenderer = cargoObject.GetComponent("SpriteRenderer") as SpriteRenderer;
//		cargoRenderer.sprite = cargoSprite;
	}

	void Unload()
	{
		ResourceProfile targetProfile = currentTarget.GetComponent("ResourceProfile") as ResourceProfile;
		if(targetProfile.FillOrder(cargoCount) == cargoCount)
		{
			currentOrder.Complete(cargoCount);
			currentOrder = null;
			cargo = "no cargo";
			cargoCount = 0;
			SetCargoSprite();
//			var cargoRenderer = cargoObject.GetComponent("SpriteRenderer") as SpriteRenderer;
//			cargoRenderer.sprite = null;
		}
		else
		{
			EnterState(State.Error);	
		}
	}

	void MoveTowardTarget()
	{
		if(currentTarget != null)
		{
			transform.position = Vector3.Lerp(transform.position, currentTarget.transform.position, speed*Time.deltaTime);
			transform.position = Vector3.MoveTowards(transform.position, currentTarget.transform.position, speed*Time.deltaTime);
		}
		else if(state == State.Moving)
		{
			transform.position = Vector3.Lerp(transform.position, MoveTarget, speed*Time.deltaTime);
			transform.position = Vector3.MoveTowards(transform.position, MoveTarget, speed*Time.deltaTime);
		}
		else 
		{
			Debug.LogError("Worker: Lost destination.");
			EnterState(State.Error);	
		}
		var pos = transform.position;
		pos.z = 0;
		transform.position = pos;
	}

	public bool AtTarget()
	{
		if(currentTarget != null)
		{
			return Vector3.Distance(transform.position, currentTarget.transform.position) <= collectionDistance;

		}
		else if(state == State.Moving)
		{
			return Vector3.Distance(transform.position, MoveTarget) <= collectionDistance;
		}
		else
		{
			Debug.LogError("Checking for target - no target.");
		}

		return false;
	}

//	public void AddOrder(Order o)
//	{
//		currentOrder = o;
//		EnterState(State.Assigned);
//	}

	void SetCargoSprite()
	{
		if(cargoCount == 0)
		{
			cargoSpriteRenderer.sprite = null;
		}
		else
		{
			cargoSpriteRenderer.sprite = Resource.GetResourceSprite(cargo);
		}
	}

	void AssignTo()
	{
		var found = UIUtil.GetObjectUnderPointer();
		if(found != null)
		{
			var buiding = found.GetComponent("Building") as Building;
			if(buiding != null)
			{
				if(buiding.AssignWorker(this))
				{
					if(owner != null)
					{
						owner.UnassignWorker(this);
						Debug.Log (this.name + " unassigned from previous owner.");
					}
					owner = buiding;
					Debug.Log (this.name + " assigned to new owner.");
				}
				else 
				{
					Debug.Log (this.name + " failed to assign to new owner.");
				}
			}
		}
	}

	public void OnSelectionChanged()
	{
		if(SelectionBox.main.selectedObjects.Contains(this.gameObject))
		{
		}
	}

	void OnGUI()
	{
		if (Event.current.type == EventType.Repaint)
		{
			var pos = Camera.main.WorldToScreenPoint( transform.position );
			Rect position = new Rect(pos.x,Camera.main.pixelHeight-pos.y, 100, 20);
			GUI.contentColor = Color.black;
			string text = state.ToString();
			GUI.Label(position,text);
		}

		if(Event.current.type == EventType.MouseDown && Input.GetMouseButtonDown(1))
		{
			if(IsSelected())
			{
				AssignTo();
			}
		}
	}

	public void Stop()
	{
		if(currentTarget != null)
		{
			currentTarget = null;
		}
		
		if(currentOrder != null)
		{
			bool hasCargo = false;
			if(cargoCount > 0)
			{
				hasCargo = true;
			}
			//cancel order
			currentOrder.Cancel(hasCargo);
			currentOrder = null;
		}
		autonomous = false;
		EnterState(State.Stopped);
	}

	public void OnStopCommand()
	{
		Stop ();
	}

	public void OnMoveCommand()
	{
		Move ();
	}

	void Move()
	{
		var pos = Input.mousePosition;
		MoveTarget = Camera.main.ScreenToWorldPoint(pos);
		MoveTarget.z = transform.position.z;
		EnterState(State.Moving);
	}

	public void OrderCanceled()
	{
		if(currentTarget != null)
		{
			currentTarget = null;
		}
		
		if(currentOrder != null)
		{
			currentOrder = null;
		}
		gameObject.layer = 11;
		EnterState(State.Idle);
	}

	public void OnGoIdleCommand()
	{
		autonomous = true;
		//cancel order
		if(currentOrder != null)
		{
			bool hasCargo = false;
			if(cargoCount > 0)
			{
				hasCargo = true;
			}
			//cancel order
			currentOrder.Cancel(hasCargo);
			currentOrder = null;
		}

		if(cargoCount > 0)
		{
			EnterState(State.IdleLoaded);
		}
		else
		{
			EnterState(State.Idle);
		}
	}

	void SellCargo ()
	{
		Order o = new Order ();
		o.amount = cargoCount;
		o.orderPlacedTime = Time.time;
		o.OrderPlacer = null;
		o.assignedWorkers.Add(this);
		o.resourceType = cargo;
		o.orderType = Order.OrderType.WTS;
		currentOrder = o;
		ResourceExchange.exchanges[0].PlaceOrder (o);
	}

	public bool AssignOrder(Order o)
	{
		if(currentOrder == null)
		{
			if(state == State.Idle)
			{
				currentOrder = o;
				o.AssignWorker(this);
				EnterState(State.Assigned);
				return true;
			}
		}
		return false;
	}
}
