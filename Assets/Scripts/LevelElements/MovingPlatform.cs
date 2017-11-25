using System.Collections;
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

	// Use this for initialization
	void Start () {
		
	}

    // Update is called once per frame
    void Update() {

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
                    
            }
        } else
        {
            if (currentPosition > 1f)
            {
                currentPosition = 0f;
            }

            if(currentPosition < 0f)
            {
                currentPosition = 1f;
            }
        }

        this.transform.position = Vector3.Lerp(startPosition.position, endPosition.position, currentPosition);

    }

    void OnDrawGizmos()
    {
        Gizmos.DrawLine(startPosition.position, endPosition.position);
    }


}
