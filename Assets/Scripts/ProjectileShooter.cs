using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour {

	public GameObject projectileType1;
	public GameObject projectileType2;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float fire1 = Input.GetAxis ("Fire1");
		if (fire1 > 0) {
			ShootProjectile (projectileType1);
		}
		float fire2 = Input.GetAxis ("Fire2");
		if (fire2 > 0) {
			ShootProjectile (projectileType2);
		}

	}

	void ShootProjectile(GameObject projectileType)
	{
		if (projectileType == null) {
			Debug.Log ("No projectile type given!");
			return;
		}

		Vector3 offset = new Vector3 (2.0f, 0, 0); // TODO: get from "Player Aim"

		GameObject projectile = Instantiate (projectileType, transform.position + offset, Quaternion.identity);
		KinematicProjectile kinBehavior = projectile.GetComponent<KinematicProjectile> ();
		if (kinBehavior != null) {
			kinBehavior.SetVelocity (new Vector2 (1.0f, 0.0f));
		}
	}
}
