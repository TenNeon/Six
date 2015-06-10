using UnityEngine;
using System.Collections;
using System.Collections.Generic;
//using Vectrosity;

// Tracks how a Building deals with a given Resource.
public class ResourceProfile : MonoBehaviour {
	public static string newLinkResourceType = "no resource type";
	public string resourceType = "no resource type";
	public int amount = 0;
	//int capacity = 0;
	int orderedAmount = 0;
	int claimedAmount = 0;
	public int exchangeUnit = 1; // when buying or selling, make orders of size exchangeUnit
	public int sellWhenAbove = 0;
	public int buyWhenBelow = 0;
	public int canFillBuyOrdersAbove = 0;
	public int canFillSellOrdersBelow = 0;
	public int capacity = 0;
	int nextLinkToCheck = 0;
	public bool canBuy = false; // I can pull this resource from others
	public bool canSell = false; // I can push this resource to others
	public bool canFillBuyOrders = false; // Buyers can pull from me
	public bool canFillSellOrders = false; // Sellers can push to me
	public bool consumes = false;
	public bool produces = false;
	bool linkDrawCheckedThisFrame = false;
	bool isDrawingLink = false;
	Vector3 endPoint;
	Vector3 startPoint;
	Vector3 mouseDownPoint;
	public Building owner;
	//VectorLine lines;
	//VectorLine linkDrawLine;
	public Material lineMaterial;
	public List<ResourceProfile> linksOut = new List<ResourceProfile>();
	public List<ResourceProfile> linksIn = new List<ResourceProfile>();
	List<Order> OrdersIPlaced = new List<Order>();
	List<Order> OrdersIFilled = new List<Order>();
	SpriteRenderer renderer;

	void Start()
	{
		owner = transform.parent.GetComponent("Building") as Building;
		if(!owner.resourceProfiles.Contains(this))
		{
			owner.resourceProfiles.Add(this);
		}
		Matchlinks();

		mouseDownPoint = Vector2.zero;
		InitLines();
		renderer = GetComponent("SpriteRenderer") as SpriteRenderer;
	}

	void Matchlinks()
	{
		if(linksOut.Count > 0)
		{
			for(int i = 0; i < linksOut.Count; i++)
			{
				if(!linksOut[i].linksIn.Contains(this))
				{
					linksOut[i].linksIn.Add(this);
				}
			}
		}
//		if(linksIn.Count > 0)
//		{
//			for(int i = 0; i < linksIn.Count; i++)
//			{
//				if(!linksIn[i].linksOut.Contains(this))
//				{
//					linksIn[i].linksOut.Add(this);
//				}
//			}
//		}
	}

	public List<string> GetPlacedOrdersStrings()
	{
		List<string> returnList = new List<string>();
		
		for(int i = 0; i < OrdersIPlaced.Count; i++)
		{
			string claimed = "claimed";
			if(OrdersIPlaced[i].orderType == null)
			{
				claimed = "unclaimed";
			}
			string assigned = "assigned";
			if(OrdersIPlaced[i].assignedWorkers.Count <= 0)
			{
				assigned = "unassigned";
			}
			returnList.Add(OrdersIPlaced[i].OrderNumber 
			               + ": " + OrdersIPlaced[i].resourceType 
			               + "("+OrdersIPlaced[i].orderPlacedTime+"): " 
			               + claimed + " " + assigned);
		}
		return returnList;
	}

	void InitLines ()
	{
		int numberOfLines = 50;
        //Vector3[] points = new Vector3[2 * numberOfLines];
        //points [0] = Vector3.zero;
        //points [1] = Vector3.zero;
        //lines = new VectorLine (resourceType + "lines", points, lineMaterial, 1, LineType.Discrete);
        //lines.lineWidth = 5f;
        //lines.sortingOrder = 0;
        //lines.sortingLayerID = 3;
        //lines.SetColor( new Color(1,0,1,0.5f) );

        //points = new Vector3[2];
        //points [0] = Vector3.zero;
        //points [1] = Vector3.zero;
        //linkDrawLine = new VectorLine (resourceType+" linkDrawLine",points, lineMaterial, 5f);
        //linkDrawLine.SetColor(Color.blue);
	}

	public void Maintain()
	{
		PlaceOrders ();

		AssignDestinations();

		AssignWorkers();
	}

	void AssignDestinations()
	{
		//look along links to find buildings that will accept deliveries for the orders
		for (int i = 0; i < OrdersIPlaced.Count; i++) {
			//Debug.Log ("order: "+OrdersIPlaced[i].resourceType+ " destination: " + OrdersIPlaced[i].GetDestination().owner.name + " " + this.owner.name + "looking for destination. In:" +linksIn.Count + " Out: " + linksOut.Count);

			if(OrdersIPlaced[i].GetDestination() == null)
			{
				if(linksOut.Count > 0)
				{
					for (int j = nextLinkToCheck; j<linksOut.Count; j++)
					{
						if(nextLinkToCheck >= linksOut.Count)
						{
							nextLinkToCheck = 0;
						}
						if(linksOut[j].AcceptOrder(OrdersIPlaced[i]))
						{
							break;
						}
					}
				}
			}
			if(OrdersIPlaced[i].GetOrigin() == null)
			{
				if(linksIn.Count > 0)
				{
					for (int j = nextLinkToCheck; j<linksIn.Count; j++)
					{
						if(nextLinkToCheck >= linksIn.Count)
						{
							nextLinkToCheck = 0;
						}
						if(linksIn[j].AcceptOrder(OrdersIPlaced[i]))
						{
							break;
						}
					}
				}
			}
		}
	}

	void PlaceOrders ()
	{
		Order o;
		//Step 2: Place orders
		int maxOrdersPerUpdate = 10;		
		while (canSell && amount - claimedAmount > sellWhenAbove && maxOrdersPerUpdate++ > 0) {
			o = new Order ();
			o.amount = exchangeUnit;
			o.orderPlacedTime = Time.time;
			o.OrderPlacer = this;
			o.resourceType = resourceType;
			o.orderType = Order.OrderType.WTS;
			OrdersIPlaced.Add (o);
			//Debug.Log (this.owner.name + " WTS " + resourceType +" @"+ Time.time);
			//ResourceExchange.exchanges[0].PlaceOrder (o);
			claimedAmount += exchangeUnit;
		}
		while (canBuy && amount + orderedAmount < buyWhenBelow && maxOrdersPerUpdate++ > 0) {
			o = new Order ();
			o.amount = exchangeUnit;
			o.orderPlacedTime = Time.time;
			o.OrderPlacer = this;
			o.resourceType = resourceType;
			o.orderType = Order.OrderType.WTB;
			OrdersIPlaced.Add (o);
			//Debug.Log (this.owner.name + " WTB " + resourceType +" @"+ Time.time);
			//ResourceExchange.exchanges[0].PlaceOrder (o);
			orderedAmount += exchangeUnit;
		}
	}

	void AssignWorkers ()
	{
		//step 3: Assign workers
		for (int i = 0; i < OrdersIPlaced.Count; i++) {
			if (OrdersIPlaced [i].OrderTaker != null) {
				if((OrdersIPlaced [i].assignedWorkers.Count * exchangeUnit) < OrdersIPlaced [i].amount)
				{
					owner.AssignWorkersToOrder(OrdersIPlaced[i]);
				}
			}
		}
	}

	public bool AcceptOrder(Order o)
	{
		//can I accept this order?
		if(o.orderType == Order.OrderType.WTS)
		{
			//if yes, assign myself to it, return true.
			if(canFillSellOrders && amount < canFillSellOrdersBelow)
			{
				if(!OrdersIFilled.Contains(o))
				{
					orderedAmount += exchangeUnit;
					OrdersIFilled.Add(o);
					//Debug.Log (this.owner.name + " taking WTS order for " + resourceType +" @"+ Time.time);
					o.OrderTaker = this;
					return true;
				}
			}
		}
		else if(o.orderType == Order.OrderType.WTB)
		{
			//if yes, assign myself to it, return true.
			if(canFillBuyOrders && amount > canFillBuyOrdersAbove)
			{
				if(!OrdersIFilled.Contains(o))
				{
					claimedAmount += exchangeUnit;
					OrdersIFilled.Add(o);
					o.OrderTaker = this;

					return true;
				}
			}
		}
		//else return false
		return false;
	}

	bool OfferToLink()
	{
		//find a linked ResourceProfile that can take this order
		//first checks to see if the link is offering, then
		//return true if handled, false if not
		//links.FindAll(x => x);
		return false;
	}

	// Reduces amount, noting that it was previously claimed
	public int Claim(int amountToClaim)
	{
		if(amountToClaim <= 0)
		{
			Debug.LogError(this.owner.name + ":" + this.resourceType +": Claim amount <= 0");
			return 0;
		}
		if(amount - amountToClaim < 0)
		{
			Debug.LogError (this.owner.name + ":" + this.resourceType +": Claim attempted when not enough resource in storage.");
			return 0;
		}
		if(claimedAmount <= 0)
		{
			Debug.LogError (this.owner.name + ":" + this.resourceType +": Claim attempted but nothing is claimed.");
			return 0;
		}
		//should this verify that the claim is valid?
		claimedAmount -= amountToClaim;
		amount -= amountToClaim;
		return amountToClaim;
	}

	// Increases amount, noting that it was previously ordered
	public int FillOrder(int amountToFill)
	{
		if(amountToFill <= 0)
		{
			Debug.LogError(this.owner.name + ":" + this.resourceType +": Fill amount <= 0");
			amountToFill = 0;
		}
		orderedAmount -= amountToFill;
		amount += amountToFill;

		return amountToFill;
	}

	public int NetAmount {
		get{ return amount + orderedAmount - claimedAmount;}
	}

	public int AvailableAmount
	{
		get{ return amount - claimedAmount;}
	}

	void AddLinkOut(ResourceProfile destination)
	{
		if (destination != this && !linksOut.Contains(destination))
		{
			destination.AddLinkIn(this);
			linksOut.Add(destination);
		}
	}

	public void AddLinkIn(ResourceProfile source)
	{
		if (source != this && !linksIn.Contains(source))
		{
			linksIn.Add(source);
		}
	}

	public void RemoveLinkOut(ResourceProfile r)
	{
		if(linksOut.Contains(r))
		{
			linksOut.Remove(r);
		}
	}

	void DrawLinks()
	{
        ////each link is drawn from origin to destination
        //for(int i = 0; i < linksOut.Count; i++)
        //{
        //    if(linksOut[i] != null)
        //    {
        //        var startPoint = transform.position;
        //        var endPoint = linksOut[i].transform.position;
				
        //        if(i*2+1 < lines.points3.Length)
        //        {
        //            lines.points3[i*2] = startPoint;
        //            lines.points3[i*2+1] = endPoint;
        //        }
        //    }
        //}
        //lines.active = true;
        //lines.Draw();
	}

	void OnGUI()
	{
        //if(SelectionBox.main.selectedObjects.Contains(this.owner.gameObject) || ResourceProfile.newLinkResourceType == resourceType || owner.hovered)
        //{
        //    renderer.enabled = true;
        //    lines.active = true;
        //}
        //else{
        //    renderer.enabled = false;
        //    lines.active = false;
        //}

        //if(!renderer.enabled)
        //{
        //    return;
        //}

        //if(isDrawingLink)
        //{
        //    DrawNewLinkLine();
        //}

        ////mouse up- finish link if started
        //if(Input.GetMouseButtonUp(0))
        //{
        //    mouseDownPoint = new Vector3(0,0,-1);
			
        //    if(isDrawingLink)
        //    {
        //        CreateLink();
        //        isDrawingLink = false;
        //        linkDrawLine.active = false;
        //        SelectionBox.Release();
        //    }	
        //}

        //DrawLinks();
	}

	void OnMouseDown()
	{
		mouseDownPoint = Input.mousePosition;
	}

	void OnMouseDrag()
	{
		if(!isDrawingLink && Input.mousePosition != mouseDownPoint)
		{
			//mouse has moved, we're actually dragging
			//SelectionBox.Suppress();
			isDrawingLink = true;
			Debug.Log (Time.time + " Start drawing link for " + owner.name + " " + resourceType);
			newLinkResourceType = resourceType;
		}
	}

	void OnMouseEnter()
	{
		var renderer = GetComponent("SpriteRenderer") as SpriteRenderer;
		owner.hovered = true;
		Color sameTypeColor = new Color(0.3f,1f,0.3f);
		if(newLinkResourceType == resourceType || newLinkResourceType == "no resource type")
		{
			renderer.color = sameTypeColor;
		}
		else 
		{
			renderer.color = Color.red;	
		}
	}
	
	void OnMouseExit()
	{
		owner.hovered = false;
		var renderer = GetComponent("SpriteRenderer") as SpriteRenderer;
		
		renderer.color = Color.white;
	}

    //void DrawNewLinkLine()
    //{
    //    linkDrawLine.active = true;
    //    Vector3 mousePos = Camera.main.ScreenToWorldPoint( Input.mousePosition );
    //    mousePos.z = 0;
    //    linkDrawLine.points3[0] = transform.position;
    //    linkDrawLine.points3[1] = mousePos;
    //    linkDrawLine.Draw();
    //}

	void CreateLink()
	{
		Debug.Log (Time.time + " Create link for " + owner.name + " " + resourceType);
		newLinkResourceType = "no resource type";

		//find building hit by mouse
		//if no building or wrong resource type, return
		var found = UIUtil.GetObjectUnderPointer();
		if (found != null)
		{
			var building = found.GetComponent("Building") as Building;
			var resourceProfile = found.GetComponent("ResourceProfile") as ResourceProfile;
			if(building != null)
			{
				ResourceProfile profile = building.resourceProfiles.Find(x => x.resourceType == resourceType);
				LinkToResourceProfile(profile);

			}
			else if(resourceProfile != null)
			{
				if(resourceProfile.resourceType == resourceType)
				{
					LinkToResourceProfile(resourceProfile);
				}
				else{
					ResourceProfile correctProfile = resourceProfile.owner.resourceProfiles.Find(x => x.resourceType == resourceType);
					LinkToResourceProfile(correctProfile);
				}
			}
			else
			{
				Debug.LogError("Tried to create link, no destination found.");	
			}
		}
		else
		{
			Debug.LogError("Tried to create link, no destination object found.");	
		}
	}

	void LinkToResourceProfile(ResourceProfile profile)
	{
		if(profile != null)
		{
			if(profile.resourceType == resourceType)
			{
				AddLinkOut(profile);
			}
			else 
			{
				Debug.LogError("Cannot create a link between different types of resources.");
			}

		}
		else
		{
			Debug.LogError("Tried to create link, no appropriate ResourceProfile found for destination building.");	
		}
	}

	public void OrderCanceled(Order o, bool cargoLost)
	{
		//is this my order? did I place this order, or take it?
		if(OrdersIPlaced.Contains(o))
		{
			//if the order is delivering something to me (place WTB), adjust my orderedAmount down
			if(o.orderType == Order.OrderType.WTB)
			{
				orderedAmount -= o.amount;
			}

			//if the order is delivering something away from me (place WTS), adjust my claimedAmount down
			if(o.orderType == Order.OrderType.WTS && !cargoLost)
			{
				claimedAmount -= o.amount;
			}
			//remove the order from my list 
			OrdersIPlaced.Remove(o);
		}
		else if(OrdersIFilled.Contains(o))
		{
			//if the order is delivering something to me (take WTS), adjust my orderedAmount down
			if(o.orderType == Order.OrderType.WTS)
			{
				orderedAmount -= o.amount;
			}

			//if the order is delivering something away from me (take WTB), adjust my claimedAmount down
			if(o.orderType == Order.OrderType.WTB && !cargoLost)
			{
				claimedAmount -= o.amount;
			}
			//remove the order from my list 
			OrdersIFilled.Remove(o);
		}
	}

	public void OnBuildingRemoved()
	{
		//tell profiles that link to me that I am being removed
		for (int i = 0; i < linksIn.Count; i++) 
		{
			linksIn[i].RemoveLinkOut(this);
		}
	}

	public void CompleteOrder(Order o)
	{
		if(OrdersIPlaced.Contains(o))
		{
			OrdersIPlaced.Remove(o);
			return;
		}

		if(OrdersIFilled.Contains(o))
		{
			OrdersIFilled.Remove(o);
		}
	}
}
