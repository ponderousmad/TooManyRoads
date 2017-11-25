using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	private PlayerInput mInput;

	// Use this for initialization
	void Start () {
		mInput = new PlayerInput (1);
	}
	
	// Update is called once per frame
	void Update () {
        bool debugInput = false;
        if(debugInput)
        {
            mInput.Aim(false);
            Debug.Log("Move: " + mInput.MoveX.ToString() + ", " + mInput.MoveY.ToString());
            Debug.Log("Fire Embiggen: " + mInput.FireEmbiggen.ToString());
            Debug.Log("Fire Debigulate: " + mInput.FireDebigulate.ToString());
        }
	}

	public Vector2 GetAim(bool snap)
	{
		return(mInput.Aim(snap));
	}
}
