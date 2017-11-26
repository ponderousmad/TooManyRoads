﻿using System.Collections;
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
	public AudioClip jumpSound;

    private bool isOnGround;
	private bool isClinging;
	private int jumpCount;
	private Rigidbody2D rigidBody;
	private float lastDir;

    private PlayerInput playerInput;

    public SmoothCD smoothVelocity = new SmoothCD();

    public Transform attachFollower;
    private Vector3 lastFollowerTransform;
	private AudioSource mAudioSource;

    void StopMoving()
    {
        if(rigidBody != null)
        {
            rigidBody.isKinematic = true;
        }
        enabled = false;
    }

	// Use this for initialization
	void Start () {
        isOnGround = true;
		isClinging = false;
		jumpCount = 0;
		rigidBody = GetComponent<Rigidbody2D> ();
		lastDir = 0.0f;

        GameRules.OnGameOver += StopMoving;
	}

    public void SetPlayerInput(PlayerInput input)
    {
        playerInput = input;
    }

	public void SetAudioSource(AudioSource audioSource)
	{
		mAudioSource = audioSource;
	}

    private void UpdateFollower()
    {
        if(attachFollower != null)
        {
            Vector3 diff = attachFollower.position - lastFollowerTransform;
            transform.position += diff;
            lastFollowerTransform = attachFollower.position;
        }
    }

	void Update() {
		bool canJump = jumpCount < maxJumps;
		if (canJump && playerInput.Jump) {
			Vector2 updatedVelocity = rigidBody.velocity;
			updatedVelocity.y = 0;
			rigidBody.velocity = updatedVelocity;

            if (isClinging && !isOnGround) {
                rigidBody.velocity = new Vector2(wallJumpImpulse.x * -lastDir, wallJumpImpulse.y);
                smoothVelocity.currentValue = rigidBody.velocity.x;
                smoothVelocity.velocity = smoothVelocity.currentValue;
//				rigidBody.AddForce (new Vector2 (wallJumpImpulse.x * -lastDir, wallJumpImpulse.y), ForceMode2D.Impulse);
			} else {
                rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpImpulse);
//				rigidBody.AddForce (new Vector2 (0.0f, jumpImpulse), ForceMode2D.Impulse);
			}
			++jumpCount;
            isOnGround = false;
			if (jumpSound != null) {
				mAudioSource.PlayOneShot (jumpSound);
			}
		}

        UpdateFollower();
	}

	// Update is called once per frame
	void FixedUpdate () {
		Vector2 moveForce = rigidBody.velocity;
		float forward = playerInput.MoveX * movementSpeed;

        smoothVelocity.smoothTime = isOnGround ? 0.1f : 0.25f;
        smoothVelocity.targetValue = forward;
        smoothVelocity.currentValue = moveForce.x;
        smoothVelocity.Update(Time.fixedDeltaTime);

        moveForce.x = smoothVelocity.currentValue;

		float newDir = 0.0f;
		newDir = Mathf.Approximately(forward, 0.0f) ? 0.0f : Mathf.Sign (forward);

		if (isClinging) { // Are we still clinging?
			if (lastDir != newDir) {
				isClinging = false;
			}
		}

        if (isClinging && !isOnGround) {
            moveForce.y = Mathf.Max(moveForce.y, slideConstantGravity);
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
                    attachFollower = collision.transform;
                    lastFollowerTransform = attachFollower.position;
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
        UpdateFollower();

        if(collision.transform == attachFollower)
        {
            attachFollower = null;
        }
		isClinging = false;
	}

	void OnClinged()
	{
		isClinging = true;
		jumpCount = 1;
	}

	void OnLanded()
	{
        isOnGround = true;
		jumpCount = 0;
	}
}
