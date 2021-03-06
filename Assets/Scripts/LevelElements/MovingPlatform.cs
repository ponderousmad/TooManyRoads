﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class MovingPlatform : MonoBehaviour {

    public Transform startPosition;
    public Transform endPosition;

    // movement speed as units per second.
    public float movementSpeed = 1f;

    public float currentPosition = 0.5f;
    public bool movingForward = true;

    public bool pingPong = true;

    protected bool hitEnd;

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    protected virtual void Update() {
        hitEnd = false;
        float length = (startPosition.position - endPosition.position).magnitude;

        float normalizedSpeed = (movementSpeed * Time.deltaTime) / length;


        if (Application.isPlaying) { 
            if (movingForward)
            {
                currentPosition += normalizedSpeed;

            } else
            {
                currentPosition -= normalizedSpeed;
            }
        }
        if (pingPong)
        {
            if (currentPosition > 1f || currentPosition < 0f)
            {
                currentPosition = Mathf.Clamp01(currentPosition);
                movingForward = !movingForward;
                hitEnd = true;
            }
        } else
        {
            if (currentPosition > 1f)
            {
                currentPosition = 0f;
                hitEnd = true;
            }

            if(currentPosition < 0f)
            {
                currentPosition = 1f;
                hitEnd = true;
            }
        }

        this.transform.position = Vector3.Lerp(startPosition.position, endPosition.position, currentPosition);

    }

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(startPosition.position, endPosition.position);
    }


}
