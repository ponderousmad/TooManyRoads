using System.Collections;
using UnityEngine;

public class Mover : MonoBehaviour {
	public Vector2 moveDirection;
	public Vector2 moveSpeed;
	public float moveTime;

	private float flip;
	private float currentTime;

	// Use this for initialization
	void Start () {
		flip = 1.0f;
		currentTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		Vector2 moveDelta = moveSpeed * Time.deltaTime * flip;

		Vector3 newPosition = transform.localPosition;
		newPosition.x += moveDirection.x * moveDelta.x;
		newPosition.y += moveDirection.y * moveDelta.y;
		transform.localPosition = newPosition;

		currentTime += Time.deltaTime;
		if (currentTime >= moveTime) {
			flip *= -1.0f;
			currentTime = 0.0f;
		}
	}
}
