using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour {

	public float health;
	private float mCurrentHealth;
	private bool mDead;

	// Use this for initialization
	void Start () {
		mCurrentHealth = health;
		mDead = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Damage(float damageAmount)
	{
		if (mDead) {
			return;
		}

		mCurrentHealth -= damageAmount;
		if (mCurrentHealth <= 0) {
			Die ();
		}
	}

	void Die()
	{
		mDead = true;
		//Debug.Log ("I died!");
        Destroy(this.gameObject);
	}
}
