﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileController : MonoBehaviour {

    public Transform gameCamera;
    public GameObject firstScene;
    public GameObject secondScene;
    public GameObject[] gameScenes;

    public float scrollValue = 0.0f;
    public float scrollSpeed = 0.0f;
    public float timeUntilIncrease = 10.0f;
    public float timeBetweenIncreases = 30.0f;
    public float scrollSpeedIncrease = 1.0f;

    private float scrollTimer = 0.0f;

    GameObject prevScene;
    GameObject currentScene;
    GameObject nextScene;
    int sceneCount = 0;

    private void CreateNewScene(GameObject gameScene)
    {
        if(prevScene)
        {
            Destroy(prevScene);
        }
        prevScene = currentScene;
        currentScene = nextScene;
        nextScene = Instantiate(gameScene, new Vector3(sceneCount * 38.0f, 0.0f), Quaternion.identity);
        ++sceneCount;
    }

    void StopMovingCamera()
    {
        enabled = false;
    }

	// Use this for initialization
	void Start () {
        if(gameCamera == null)
        {
            gameObject.SetActive(false);
            return;
        }

        if(gameScenes.Length == 0)
        {
            gameObject.SetActive(false);
            return;
        }

        if(firstScene)
        {
            CreateNewScene(firstScene);
        } else
        {
            CreateNewScene(gameScenes[Random.Range(0, gameScenes.Length)]);
        }

        if(secondScene)
        {
            CreateNewScene(secondScene);
        } else
        {
            CreateNewScene(gameScenes[Random.Range(0, gameScenes.Length)]);
        }
	}
	
	// Update is called once per frame
	void Update () {
        scrollTimer += Time.deltaTime;
        if(scrollTimer > timeUntilIncrease)
        {
            scrollSpeed += scrollSpeedIncrease;
            scrollTimer -= timeUntilIncrease;
            timeUntilIncrease = timeBetweenIncreases;
        }

        scrollValue += scrollSpeed * Time.deltaTime;
        int scrollPixels = Mathf.RoundToInt(scrollValue * 96.0f);
        gameCamera.position = new Vector3(scrollPixels / 96.0f, gameCamera.position.y, gameCamera.position.z);

        if(scrollValue > ((sceneCount-1) * 38.0f))
        {
            CreateNewScene(gameScenes[Random.Range(0, gameScenes.Length)]);
        }
	}
}
