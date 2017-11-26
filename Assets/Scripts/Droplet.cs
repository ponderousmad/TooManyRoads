using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Droplet : MonoBehaviour {

	public float minSplashSize = 0.2f;
	public Vector2 splashForceMultiplier = new Vector2(10.0f, 10.0f);

	private Embiggener mEmbiggener;

	// Use this for initialization
	void Start () {
		mEmbiggener = GetComponent<Embiggener> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnCollisionEnter2D(Collision2D collider)
	{
		GameObject other = collider.gameObject;
		if (other.GetComponent<Droplet> () != null) {
			return; // droplets don't collide with other droplets
		}

		float extraSplitSize = mEmbiggener.mCurrentScaleValue - 0.1f;
		while (extraSplitSize > 0) {
			float maxSize = Mathf.Max (minSplashSize, extraSplitSize / 2);
			float nextSize = Mathf.Min (extraSplitSize, Random.Range (minSplashSize, maxSize));
			extraSplitSize -= nextSize;
			GameObject newDroplet = Instantiate (this.gameObject, transform.position, Quaternion.identity);
			Rigidbody2D body = newDroplet.GetComponent<Rigidbody2D> ();
			if (body != null) {
				Vector2 f = Random.insideUnitCircle;
				f.x *= splashForceMultiplier.x;
				f.y = 0.5f * (f.y + 1) * splashForceMultiplier.y;
				body.AddForce (f);
			}
			Embiggener e = newDroplet.GetComponent<Embiggener> ();
			if (e != null) {
				e.SetScaleValue (nextSize);
			}
		}
			
		BroadcastMessage("Collision", collider.collider);

		Destroy (this.gameObject);
	}
}
