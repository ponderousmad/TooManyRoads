using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysCharacterController : MonoBehaviour {
	public float movementSpeed = 5.0f;
	public float jumpImpulse = 5.0f;
	public float slope = 45.0f; // in degrees
	public int maxJumps = 1;

	private bool isGrounded;
	private int jumpCount;
	private Rigidbody2D rigidBody;

	// Use this for initialization
	void Start () {
		isGrounded = false;
		jumpCount = 0;
		rigidBody = GetComponent<Rigidbody2D> ();
	}

	void Update()
	{
		if(Input.GetButtonDown("Jump"))
		{
			rigidBody.AddForce(new Vector2(0.0f, jumpImpulse), ForceMode2D.Impulse);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		Vector2 moveForce = rigidBody.velocity;
		float forward = Input.GetAxis ("Horizontal") * movementSpeed;
		moveForce.x = forward;

		rigidBody.velocity = moveForce;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		//ContactPoint2D[] contacts;
		//collision.GetContacts(contacts);

		//if (contacts.Length > 0) {
		//	if (Vector2.Dot(contacts[0].normal, Vector2.up) > Mathf.Cos(Mathf.Deg2Rad * slope)) {
		//		OnLanded();
		//	}
		//}
	}

	void OnLanded()
	{
		isGrounded = true;
		jumpCount = 0;
	}
}
