using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToTheFuture : MonoBehaviour {
	public float nextSceneTime = 3.0f;

	private float currentSceneTime = 0.0f;
	
	// Update is called once per frame
	void Update () {
		currentSceneTime += Time.deltaTime;
		if (currentSceneTime > nextSceneTime) {
			SceneManager.LoadScene (0);
		}
	}
}
