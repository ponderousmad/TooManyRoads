using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmbiggeningCollider : CollisionResponse {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
		
	public override void Collision(Collider2D other)
	//void OnCollision(Collider2D other)
	//void OnCollision(Collider2D other)
	//void OnCollisionEnter2D(Collision2D coll)
	{
		Debug.Log ("collision!");
		//Collider2D other = coll.collider;
		//Collider2D other = (Collider2D)coll;
		//Collider2D other = new Collider2D();
		Embiggener embiggener = other.GetComponent<Embiggener>();
		if(embiggener != null) 
		{
			embiggener.Embiggen(0.1f);
		}
	}
}
