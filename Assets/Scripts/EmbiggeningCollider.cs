using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmbiggeningCollider : CollisionResponse {

	public float embiggenAmount = 0.1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void Collision(Collider2D other)
	{
		Embiggener embiggener = other.GetComponent<Embiggener>();
		if(embiggener != null) 
		{
			embiggener.Embiggen(embiggenAmount);
		}
	}
}
