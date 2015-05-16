using UnityEngine;
using System.Collections;

public class CircularGauge : MonoBehaviour {
	public ResourceProfile owner;

	void Start()
	{
		owner = transform.parent.GetComponent("ResourceProfile") as ResourceProfile;
	}

	// Update is called once per frame
	void Update () {
		GetComponent<Renderer>().enabled = owner .GetComponent<Renderer>().enabled;
		GetComponent<Renderer>().material.SetFloat("_Cutoff", Mathf.InverseLerp(owner.capacity, 0, owner.amount));
	}
}
