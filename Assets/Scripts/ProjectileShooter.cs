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
    public float selfEmbiggen = 0.1f;

	private float mShootTimer;

	private AmmoController mAmmoControl;
    private PlayerInput mPlayerInput;

	// Use this for initialization
	void Start () {
		mShootTimer = 0;
		mAmmoControl = GetComponent<AmmoController> ();
	}

    public void SetInput(PlayerInput input)
    {
        mPlayerInput = input;
    }
	
	// Update is called once per frame
	void Update () {
		if (mShootTimer > 0) {
			mShootTimer -= Time.deltaTime;
		}
        
        if(!CanShoot())
        {
            return;
        }
        Embiggener embiggener = transform.parent.GetComponent<Embiggener>();
        if(embiggener != null) 
        {
            if(mPlayerInput.SelfEmbiggen)
            {
                Debug.Log("Embiggen self");
                embiggener.Embiggen(selfEmbiggen);
                SpendProjectile();
            }
            else if(mPlayerInput.SelfDebigulate)
            {
                Debug.Log("Debigulate self");
                embiggener.Embiggen(-selfEmbiggen);
                SpendProjectile();
            }
        }
        if (mPlayerInput.FireEmbiggen) {
            ShootProjectile (projectileType1);
        }
        if (mPlayerInput.FireDebigulate) {
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

		Vector2 aim = mPlayerInput.Aim(true);
		Debug.Log ("aim: " + aim);
		Vector3 offset = new Vector3 (aim.x, aim.y, 0); // TODO: data drive? consider player scale too

		GameObject projectile = Instantiate (projectileType, transform.position + offset, Quaternion.identity);
		KinematicProjectile kinBehavior = projectile.GetComponent<KinematicProjectile> ();
		if (kinBehavior != null) {
			kinBehavior.SetVelocity (aim * projectileSpeed);
			kinBehavior.SetInstigator (gameObject);
		}
        SpendProjectile();
	}

    private void SpendProjectile()
    {
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
}
