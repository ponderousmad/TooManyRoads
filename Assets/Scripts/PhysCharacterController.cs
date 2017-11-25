using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysCharacterController : MonoBehaviour {
	public float movementSpeed = 5.0f;
	public float jumpImpulse = 5.0f;
	public float slope = 45.0f; // in degrees
	public float wallSlope = 15.0f;
	public int maxJumps = 2;

	// Debug variables
	public Vector2 debugVelocity;
	public bool debugIsClinging;

	private bool isClinging;
	private int jumpCount;
	private Rigidbody2D rigidBody;
	private float lastDir;

	// Use this for initialization
	void Start () {
		isClinging = false;
		jumpCount = 0;
		rigidBody = GetComponent<Rigidbody2D> ();
		lastDir = 0.0f;
	}

	void Update() {
		bool canJump = jumpCount < maxJumps;
		if (canJump && Input.GetButtonDown("Jump")) {
			Vector2 updatedVelocity = rigidBody.velocity;
			updatedVelocity.y = 0;
			rigidBody.velocity = updatedVelocity;
			rigidBody.AddForce(new Vector2(0.0f, jumpImpulse), ForceMode2D.Impulse);
			++jumpCount;
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		Vector2 moveForce = rigidBody.velocity;
		float forward = Input.GetAxis ("Horizontal") * movementSpeed;
		moveForce.x = forward;

		if (isClinging) { // Are we still clinging?
			
		}

		lastDir = Mathf.Sign (forward);

		debugVelocity = moveForce;
		rigidBody.velocity = moveForce;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		ContactPoint2D[] contacts = new ContactPoint2D[collision.contacts.Length];
		collision.GetContacts(contacts);

		if (contacts.Length > 0) {
			if (Vector2.Dot (contacts [0].normal, Vector2.up) > Mathf.Cos (Mathf.Deg2Rad * slope)) {
				OnLanded ();

				if (collision.gameObject.CompareTag ("Mover")) {
					transform.parent = collision.gameObject.transform;
				}
			} else {
				if (Vector2.Dot(contacts[0].normal, Vector2.right * lastDir) > Mathf.Cos(Mathf.Deg2Rad * wallSlope)) {
					isClinging = true;
				}
			}
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		transform.parent = null;
	}

	void OnLanded()
	{
		jumpCount = 0;
	}
}
