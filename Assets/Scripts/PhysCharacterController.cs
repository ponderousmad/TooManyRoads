using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysCharacterController : MonoBehaviour {
	public float movementSpeed;
	public float jumpImpulse;
	public LayerMask groundSurfaces;
	public Transform basePoint;
	public float groundCheckRadius;

	private bool isGrounded = false;
	private Rigidbody2D rigidBody;

	// Use this for initialization
	void Start () {
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
		isGrounded = Physics2D.OverlapCircle (basePoint.position, groundCheckRadius);
		Debug.Log ("IS GROUNDED: " + isGrounded);

		Vector2 moveForce = rigidBody.velocity;
		float forward = Input.GetAxis ("Horizontal") * movementSpeed;
		moveForce.x = forward;

		rigidBody.velocity = moveForce;
	}
}
