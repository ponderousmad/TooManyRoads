using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyForceZone : MonoBehaviour {

	public Vector2 force = new Vector2(0.0f, 0.0f);

	// Use this for initialization
	void Start () {
		ParticleSystem ps = GetComponentInChildren<ParticleSystem> ();
		if (ps != null) {
			ParticleSystem.MainModule main = ps.main;
			ParticleSystem.MinMaxCurve startSpeed = main.startSpeed;
			startSpeed.constant = startSpeed.constant * force.magnitude;
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		ApplyForce (collider);
	}

	void OnTriggerStay2D(Collider2D collider)
	{
		ApplyForce (collider);
	}

	private void ApplyForce(Collider2D collider)
	{
		Rigidbody2D otherRigidbody = collider.attachedRigidbody;
		if (otherRigidbody != null) {
			otherRigidbody.AddForce (force);
		}
	}
}
