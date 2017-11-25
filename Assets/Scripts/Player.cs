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
		
	}

	public Vector2 GetAim(bool snap)
	{
		return(mInput.Aim(snap));
	}
}
