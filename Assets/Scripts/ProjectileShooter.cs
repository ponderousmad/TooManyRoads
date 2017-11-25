using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour {

	public GameObject projectileType1;
	public GameObject projectileType2;

	public float projectileSpeed = 10.0f;
	public float fireRate = 1.0f;

	private float mShootTimer;

	// Use this for initialization
	void Start () {
		mShootTimer = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (mShootTimer > 0) {
			mShootTimer -= Time.deltaTime;
		}

		float fire1 = Input.GetAxis ("Fire1");
		if (fire1 > 0 && CanShoot()) {
			ShootProjectile (projectileType1);
		}
		float fire2 = Input.GetAxis ("Fire2");
		if (fire2 > 0 && CanShoot()) {
			ShootProjectile (projectileType2);
		}

	}

	void ShootProjectile(GameObject projectileType)
	{
		if (projectileType == null) {
			Debug.Log ("No projectile type given!");
			return;
		}

		Vector3 offset = new Vector3 (2.0f, 0, 0); // TODO: get from "Player Aim. Also consider player scale."

		GameObject projectile = Instantiate (projectileType, transform.position + offset, Quaternion.identity);
		KinematicProjectile kinBehavior = projectile.GetComponent<KinematicProjectile> ();
		if (kinBehavior != null) {
			kinBehavior.SetVelocity (new Vector2 (1.0f, 0.0f) * projectileSpeed);
		}

		mShootTimer = (1 / fireRate);
	}

	private bool CanShoot()
	{
		if (mShootTimer > 0) {
			return(false);
		}

		return(true);
	}
}
