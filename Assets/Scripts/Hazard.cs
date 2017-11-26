using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour {

	public float damageAmount = 1.0f;
    public bool force = false;
	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		
	}
    void OnTriggerEnter2D(Collider2D collision)
    {
        DamageController dc = collision.gameObject.GetComponent<DamageController>();
        if (dc != null)
        {
            dc.Damage(damageAmount, force);
        }
    }

    void OnTriggerStay2D(Collider2D collision)
    {
        DamageController dc = collision.gameObject.GetComponent<DamageController>();
        if (dc != null)
        {
            dc.Damage(damageAmount, force);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
	{
		DamageController dc = collision.gameObject.GetComponent<DamageController>();
		if(dc != null)
		{
			dc.Damage(damageAmount, force);
		}
	}
}
