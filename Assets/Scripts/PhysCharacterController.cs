using System.Collections;
using UnityEngine;

public class PhysCharacterController : MonoBehaviour {
	public float movementSpeed = 5.0f;
	public float jumpImpulse = 5.0f;
	public float slope = 45.0f; // in degrees
	public float wallSlope = 15.0f;
	public int maxJumps = 2;
	public float slideConstantGravity = -2.0f;
	public Vector2 wallJumpImpulse;

	// Debug variables
	public Vector2 debugVelocity;
	public bool debugIsClinging;
	public Vector2 debugContactNormal;
	public float debugLastDir;
	public float debugRightDot;
	public float debugRightDotCheck;

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

			if (isClinging) {
				rigidBody.AddForce (new Vector2 (wallJumpImpulse.x * -lastDir, wallJumpImpulse.y), ForceMode2D.Impulse);
			} else {
				rigidBody.AddForce (new Vector2 (0.0f, jumpImpulse), ForceMode2D.Impulse);
			}
			++jumpCount;
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		Vector2 moveForce = rigidBody.velocity;
		float forward = Input.GetAxisRaw ("Horizontal") * movementSpeed;

		moveForce.x = forward;
		float newDir = 0.0f;
		newDir = Mathf.Approximately(forward, 0.0f) ? 0.0f : Mathf.Sign (forward);

		if (isClinging) { // Are we still clinging?
			if (lastDir != newDir) {
				isClinging = false;
			}
		}

		if (isClinging) {
			moveForce.y = slideConstantGravity;
		}
		debugIsClinging = isClinging;

		lastDir = newDir;
		debugLastDir = lastDir;

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
				debugRightDot = Vector2.Dot(contacts[0].normal, Vector2.right * -lastDir);
				float rightDotCheck = Mathf.Cos (Mathf.Deg2Rad * wallSlope);
				debugRightDotCheck = rightDotCheck;
				if (Vector2.Dot(contacts[0].normal, Vector2.right * -lastDir) > rightDotCheck) {
					OnClinged ();
				}
			}

			debugContactNormal = contacts [0].normal;
		}
	}

	void OnCollisionExit2D(Collision2D collision)
	{
		transform.parent = null;
		isClinging = false;
	}

	void OnClinged()
	{
		isClinging = true;
		jumpCount = 1;
	}

	void OnLanded()
	{
		jumpCount = 0;
	}
}
