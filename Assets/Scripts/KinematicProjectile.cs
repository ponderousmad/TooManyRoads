using UnityEngine;
using System.Collections;

public class KinematicProjectile : MonoBehaviour {

    private Vector3 mVelocity = new Vector3();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = transform.position + mVelocity * Time.deltaTime;
	}
}
