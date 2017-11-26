using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lock2DRigidBodyToPosition : MonoBehaviour {

    public GameObject target;
    public Rigidbody2D body;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        body.MovePosition(target.transform.position);
       // body.MoveRotation(target.transform.rotation.z);

    }
}
