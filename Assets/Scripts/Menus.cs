using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menus : MonoBehaviour {

	public void Play()
	{
		System.IO.File.WriteAllText(
			@"/Users/agnomen/Documents/workspace/git/TooManyRoads/Assets/Inputs.txt",
			PlayerInput.AllDefinitions()
		);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
