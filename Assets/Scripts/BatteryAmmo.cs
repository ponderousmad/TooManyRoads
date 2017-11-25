using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatteryAmmo : AmmoController {

	public float maxAmmo = 5.0f;
	public float rechargeRate = 1.0f;
	public bool startFull = true;

	private float mCurrentAmmo;

	// Use this for initialization
	void Start () {
		if (startFull) {
			mCurrentAmmo = maxAmmo;
		} else {
			mCurrentAmmo = 0;
		}
	}

	// Update is called once per frame
	void Update () {
		if (mCurrentAmmo < maxAmmo) {
			mCurrentAmmo += rechargeRate * Time.deltaTime;
		}
	}

	public override bool HasAmmo(float amount)
	{
		return(mCurrentAmmo >= amount);
	}

	public override void UseAmmo(float amount)
	{
		mCurrentAmmo = Mathf.Max (0, mCurrentAmmo - amount);
	}

	public override float GetAmmo()
	{
		return(mCurrentAmmo);
	}
}
