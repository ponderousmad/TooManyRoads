using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileShooter : MonoBehaviour {

	public GameObject projectileType1;
	public GameObject projectileType2;
	public bool snapAim = true;

	public float projectileSpeed = 10.0f;
	public float fireRate = 1.0f;
	public float requiredAmmo = 1.0f;

	private float mShootTimer;

	private AmmoController mAmmoControl;

	// Use this for initialization
	void Start () {
		mShootTimer = 0;
		mAmmoControl = GetComponent<AmmoController> ();
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
		Debug.Log ("shooting projectile");
		if (projectileType == null) {
			Debug.Log ("No projectile type given!");
			return;
		}

		Vector2 aim = GetPlayerAim ();
		Debug.Log ("aim: " + aim);
		Vector3 offset = new Vector3 (aim.x, aim.y, 0); // TODO: data drive? consider player scale too

		GameObject projectile = Instantiate (projectileType, transform.position + offset, Quaternion.identity);
		KinematicProjectile kinBehavior = projectile.GetComponent<KinematicProjectile> ();
		if (kinBehavior != null) {
			kinBehavior.SetVelocity (aim * projectileSpeed);
		}

		mShootTimer = (1 / fireRate);
		mAmmoControl.UseAmmo (requiredAmmo);
	}

	private bool CanShoot()
	{
		if (mShootTimer > 0) {
			return(false);
		}
			
		if (mAmmoControl != null && !mAmmoControl.HasAmmo(requiredAmmo)) {
			Debug.Log ("no ammo");
			return(false);
		}

		return(true);
	}

	private Vector2 GetPlayerAim()
	{
		if (transform.parent != null) {
			if (transform.parent.gameObject != null) {
				Player p = transform.parent.gameObject.GetComponent<Player> ();
				if (p != null) {
					Debug.Log ("got aim from player");
					return(p.GetAim (snapAim));
				}
			}
		}

		return(new Vector2(0.0f, 0.0f));
	}
}
