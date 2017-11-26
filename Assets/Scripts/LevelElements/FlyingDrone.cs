using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingDrone : MonoBehaviour {

    public float forwardForce;

    private Vector2 forceDirection;

    private void randomizeDirection()
    {
        this.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        float randomAngle = Random.Range(0, Mathf.PI * 2f);
        forceDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));
    }

    private void randomizeDirection(Vector2 hitNormal)
    {
     //   float baseAngle = Vector2.Angle(hitNormal, Vector2.right);
      //  float randomAngle = Random.Range(baseAngle - Mathf.PI / 4, baseAngle + Mathf.PI / 4);
      //   forceDirection = new Vector2(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle));

        forceDirection = hitNormal;
    }

	// Use this for initialization
	void Start () {
        randomizeDirection();

    }
	
	// Update is called once per frame
	void FixedUpdate () { 
        this.GetComponent<Rigidbody2D>().AddForce(forceDirection * forwardForce * Time.deltaTime);
        this.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(forceDirection.y, forceDirection.x) * Mathf.Rad2Deg);
	}

   /* void OnCollisionEnter2D(Collision2D collider)
    {
        randomizeDirection();
    }*/
    void OnCollisionStay2D(Collision2D collision)
    {
        Vector2 norm = collision.contacts[0].normal;
        randomizeDirection(norm);
        FixedUpdate();
    }
}
