using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {

    public Transform gameCamera;
    public GameObject gameScene;

    public float scrollValue = 0.0f;
    public float scrollSpeed = 1.0f;
    public float timeBetweenIncreases = 30.0f;
    public float scrollSpeedIncrease = 1.0f;

    private float scrollTimer = 0.0f;

	// Use this for initialization
	void Start () {
        if(gameCamera == null)
        {
            gameObject.SetActive(false);
            return;
        }

        if(gameScene == null)
        {
            gameObject.SetActive(false);
            return;
        }

        GameObject newScene = Instantiate(gameScene);
        if(newScene == null)
        {
            gameObject.SetActive(false);
            return;
        }

        GameObject secondScene = Instantiate(gameScene, new Vector3(38.0f, 0.0f, 0.0f), Quaternion.identity);
        if(secondScene == null)
        {
            gameObject.SetActive(false);
            return;
        }
	}
	
	// Update is called once per frame
	void Update () {
        scrollTimer += Time.deltaTime;
        if(scrollTimer > timeBetweenIncreases)
        {
            scrollSpeed += scrollSpeedIncrease;
            scrollTimer -= timeBetweenIncreases;
        }

        scrollValue += scrollSpeed * Time.deltaTime;
        int scrollPixels = Mathf.RoundToInt(scrollValue * 96.0f);
        gameCamera.position = new Vector3(scrollPixels / 96.0f, gameCamera.position.y, gameCamera.position.z);
	}
}
