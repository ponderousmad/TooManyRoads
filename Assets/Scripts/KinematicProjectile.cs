using UnityEngine;
using System.Collections;

public class KinematicProjectile : MonoBehaviour {

	public bool gravityEnabled = false;

	private GameObject mInstigator;
	private Vector2 mVelocity = new Vector2();

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update() {
		if (gravityEnabled) {
			mVelocity += Time.deltaTime * new Vector2(Physics.gravity.x, Physics.gravity.y);
		}
		Vector2 pos2D = new Vector2 (transform.position.x, transform.position.y);
		RaycastHit2D result = Physics2D.Raycast(pos2D, mVelocity, mVelocity.magnitude * Time.deltaTime);
		Collider2D hitObject = result.collider;
		if (hitObject == null || hitObject.gameObject == mInstigator) {
			transform.position = transform.position + new Vector3(mVelocity.x, mVelocity.y, 0) * Time.deltaTime;
		} else {
			CollisionResponse cr = this.gameObject.GetComponent<CollisionResponse>();
			if (cr != null) {
				cr.Collision (hitObject);
			}
			Destroy (this.gameObject);
		}
	}

	public void SetVelocity(Vector2 vel)
	{
		mVelocity = vel;
	}

	public void EnableGravity(bool enabled)
	{
		gravityEnabled = enabled;
	}

	public void SetInstigator(GameObject instigator)
	{
		Debug.Log ("Instigator set to " + instigator.GetType ().FullName);
		mInstigator = instigator;
	}
}
