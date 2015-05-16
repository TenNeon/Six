using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Turret : Building {
	public float fireRate = .1f;
	float timeLastFired = 0f;
	public Transform bullet;
	public float maxSpread = 0.25f;
	public float spreadPerShot = 0.20f;
	public float spreadReduction = .4f;
	public int munitions = 50;
	public int reloadAmount = 14;
	int amountToReload = 0;
	public float trackingRange = 4.95f;
	public float rotationSpeed = 1f;
	public float energy = 0;
	float minimumEnergyBeforeDischarge = 50f;
	float energyConsumedPerShot = 0.25f;
	public float bulletSpeed = 200f;
	public float bulletLife = 0.5f;
	public float bulletSpeedVariation = 0.3f;
	int upgradeKits = 1;
	float upgradeDuration = 5f;
	float upgradeProgress = 0;
	public bool idle = false;
	bool isDischargingCell = false;
	Enemy currentTarget;
	// Use this for initialization
	void Start () {
		InvokeRepeating("Reload",0,5f);
		BuildingStart();
	}
	
	// Update is called once per frame
	void Update () {
		if (idle)
		{
			return;
		}
		SpinShoot ();
		TrackTarget();
		energy *= 0.999f;
	}

	void Tick()
	{
		if(!isDischargingCell && energy < minimumEnergyBeforeDischarge)
		{
			var fullCells = resourceProfiles.Find(x => x.name == "Energy Cell (full)").GetComponent("ResourceProfile") as ResourceProfile;
			if(fullCells!=null && fullCells.amount > 0)
			{
				StartCoroutine("DischargeCell");
			}
		}
		var munitionsPackages = resourceProfiles.Find(x => x.name == "Munitions").GetComponent("ResourceProfile") as ResourceProfile;
		if (munitionsPackages.amount >= 1 && munitions < 50) 
		{
			munitionsPackages.amount -= 1;
			munitions += 100;
		}
	}

	IEnumerator DischargeCell()
	{
		ResourceProfile fullCells = resourceProfiles.Find(x => x.name == "Energy Cell (full)").GetComponent("ResourceProfile") as ResourceProfile;
		ResourceProfile emptyCells = resourceProfiles.Find(x => x.name == "Energy Cell (empty)").GetComponent("ResourceProfile") as ResourceProfile;
		
		fullCells.amount -= 1;
		for (int i = 0; i < 100; i++) {
			energy += 1;
			yield return new WaitForSeconds(0.02F);
		}
		emptyCells.amount += 1;
	}

	void SpinShoot()
	{
		transform.Rotate(new Vector3(0,0,rotationSpeed));
		Fire ();
	}

	void Fire()
	{
		float modifiedBonusPowerConsumedPerShot = energyConsumedPerShot;
		float modifiedFireRate = (fireRate*20f/munitions);
		if(bullet != null && munitions > 0 && timeLastFired + modifiedFireRate <= Time.time)
		{
			var position = transform.position;
			var rotation = transform.rotation;
			var newBullet = Instantiate(bullet,position,rotation) as Transform;

			Rigidbody2D body = newBullet.GetComponent("Rigidbody2D") as Rigidbody2D;
			Bullet bulletScript = newBullet.GetComponent("Bullet") as Bullet;
			bulletScript.maxAge = bulletLife;
			if(body == null)
			{
				return;
			}
			var rot3D = transform.rotation*Vector3.up;

			var modBulletSpeed = Random.Range(bulletSpeed*(1-bulletSpeedVariation),bulletSpeed*(1+bulletSpeedVariation));
			var impulseVector = Vector3.Scale(rot3D, new Vector3(modBulletSpeed,modBulletSpeed,0));
			body.AddForce(new Vector2(impulseVector.x,impulseVector.y));

			if(bulletScript != null)
			{
				bulletScript.power = 1 + modifiedBonusPowerConsumedPerShot;
				energy -= modifiedBonusPowerConsumedPerShot;
			}

			timeLastFired = Time.time;
			munitions -= 1;
		}
	}

	void Reload()
	{
		amountToReload += reloadAmount;
		StartCoroutine("AddMunitions");
	}


	IEnumerator AddMunitions()
	{
		while(amountToReload > 0)
		{
			munitions += 1;
			amountToReload -= 1;
			yield return null;
		}
	}

	void TrackTarget()
	{
		if(currentTarget != null && currentTarget.alive)
		{
			if(AlignedWithTarget() == true)
			{
				Fire ();
			}
			TurnTowardTarget();
		}
		else
		{
			PickTarget();
		}
	}

	//should eventually pick a target that is either close or rotationally close
	//currently picks first in list, which should have similar results
	void PickTarget()
	{
		foreach (Transform child in transform)
		{
			var scanner = child.GetComponent("EnemyScanner") as EnemyScanner;
			if(scanner != null)
			{
				currentTarget = scanner.PickTarget();
				return;
			}
		}
	}

	bool AlignedWithTarget ()
	{
		Vector3 fwd = transform.TransformDirection(Vector3.up);
		Debug.DrawRay(transform.position, fwd*trackingRange);
		var layerMask = 1 << 8;
		if(Physics2D.Raycast(transform.position, fwd, trackingRange,layerMask))
		{
			return true;
		}
		else{
			return false;
		}
	}

	void TurnTowardTarget()
	{
		var targetPos = currentTarget.transform.position;
		transform.eulerAngles = new Vector3(0,0,
		                                              Mathf.Atan2((targetPos.y - transform.position.y)
		            ,(targetPos.x - transform.position.x))*Mathf.Rad2Deg - 90);

	}

	void PlaceholderUpgrade()
	{
		if(upgradeKits > 0 && upgradeProgress == 0)
		{
			upgradeKits -= 1;
		}
		StartCoroutine("UpgradeProgressBar");
	}

	IEnumerator UpgradeProgressBar()
	{
		int framesSpentInUpgradeLoop = 0;
		float lastUpdateTime = Time.time;
		upgradeProgress = 0.00001f;
		while(upgradeProgress < upgradeDuration)
		{
			framesSpentInUpgradeLoop++;
			upgradeProgress += Time.time - lastUpdateTime;
			lastUpdateTime = Time.time;
			yield return new WaitForSeconds(1f);
		}
		Debug.Log("Frames spent in upgrade loop: " + framesSpentInUpgradeLoop);
		upgradeProgress = 0f;
		UpgradeComplete();
	}

	void UpgradeComplete()
	{
		bulletSpeed *= 2f;
		bulletLife *= 0.5f;
	}

	Rect windowRect = new Rect(10, 10, 120, 100);
	void OnGUI()
	{
		if(SelectionBox.main.selectedObjects.Contains(this.gameObject))
		{
			windowRect = GUILayout.Window(0, windowRect, DoMyWindow, "Turret", GUILayout.Width(120));
		}
	}

	void DoMyWindow(int windowID)
	{
		GUIStyle buttonStyle = GUI.skin.GetStyle("Button");
		buttonStyle.wordWrap = true;

		GUILayout.Label ("Energy: " + energy.ToString("F0") +"("+ energyConsumedPerShot.ToString("F1")+")");
		GUILayout.Label ("Munitions: " + munitions);
		GUILayout.Label ("Upgrade Kits: " + upgradeKits);
		GUI.enabled = upgradeKits > 0 && upgradeProgress == 0;
		if(upgradeProgress > 0)
		{
			GUILayout.BeginArea(new Rect(10,100,100,25));
			Rect temp;
			var size = new Vector2(100,20);
			//draw the background
			if(ProgressBar.main.backgroundTexture != null){
				temp = new Rect(0,0,size.x,size.y);
				GUI.DrawTexture(temp, ProgressBar.main.backgroundTexture);
			}
			//draw the bar
			if(ProgressBar.main.texture != null)
			{
				temp = new Rect(2,2,size.x*(upgradeProgress/upgradeDuration)-8,size.y-4);
				GUI.DrawTexture(temp, ProgressBar.main.texture);
			}
			//draw the foreground
			if(ProgressBar.main.foregroundTexture != null)
			{
				temp = new Rect(0,0,size.x,size.y);
				GUI.DrawTexture(temp, ProgressBar.main.foregroundTexture);
			}
			GUILayout.EndArea();
		}
		else if(GUILayout.Button("Upgrade Bullet Velocity",buttonStyle))
		{
			PlaceholderUpgrade();
		}
		GUI.DragWindow();
	}
}
