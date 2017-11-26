using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AmmoController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public abstract bool HasAmmo (float amount);
	public abstract void UseAmmo (float amount);
	public abstract float GetAmmo();
    public abstract float PowerLevel();
}
