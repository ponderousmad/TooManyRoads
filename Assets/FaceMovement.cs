using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceMovement : MonoBehaviour {
    public Player player;

    public SmoothCD turnDirection = new SmoothCD();

	// Use this for initialization
	void Start () {
        if(player == null)
        {
            enabled = false;
        }

        turnDirection.smoothTime = 0.1f;
	}
	
	// Update is called once per frame
	void Update () {
        float aimx = player.Input.LastMoveDirection;

        if(aimx > 0.0f)
        {
            turnDirection.targetValue = 90.0f;
        }
        else if(aimx < 0.0f)
        {
            turnDirection.targetValue = -90.0f;
        }

        turnDirection.Update(Time.deltaTime);

        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, turnDirection.currentValue, transform.localEulerAngles.z);
	}
}
