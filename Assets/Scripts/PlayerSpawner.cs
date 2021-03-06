﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {
	public GameObject playerPrefab;
	public int playerId;

	private bool spawn = false;

    public Camera camera;
    private GameObject player;

    public PlayerDisplay display;

    public int numSpawns = 3;


	// Use this for initialization
	void Awake () {
		string playerConfig = PlayerPrefs.GetString ("Player" + playerId, "");
		if (playerConfig.CompareTo("") != 0) {
			spawn = true;
		}
	}

	void Start () {
        if (!spawn || camera == null) {
            if(display)
            {
                display.gameObject.SetActive(false);
            }
			Destroy (this.gameObject);
			return;
		}
	}

    void Update()
    {
        if(player == null)
        {
            if(numSpawns == 0)
            {
                display.SetLives(0);
                enabled = false;
            } else
            {
                SpawnPlayer();
            }
        }
    }

    void SpawnPlayer()
    {
        // Find a spawn point
        GameObject[] respawns = GameObject.FindGameObjectsWithTag("Respawn");

        float cameraHorizontalSize = (camera.orthographicSize * 16.0f) / 9.0f;

        GameObject bestSpawn = null;
        float bestX = -50000.0f;

        foreach(GameObject go in respawns)
        {
            float distanceFromCamera = go.transform.position.x - camera.transform.position.x;
            if((distanceFromCamera > (-cameraHorizontalSize / 3.0f)) && (distanceFromCamera < (2.0 * cameraHorizontalSize / 3.0f)) && distanceFromCamera > bestX)
            {
                bestSpawn = go;
                bestX = distanceFromCamera;
            }
        }

        if(bestSpawn != null)
        {
            display.SetLives(numSpawns);
            player = Instantiate(playerPrefab, bestSpawn.transform.position, Quaternion.identity);
            Debug.Log("Spawned Player:");
            Debug.Log(player);

            player.GetComponent<Player>().SetID(playerId);

			//Destroy(bestSpawn);

            if(display)
            {
                foreach(var ammo in player.GetComponentsInChildren<AmmoController>())
                {
                    display.ammoController = ammo;
                }
            }

            --numSpawns;
        }
        else
        {
            Debug.Log("Failed to spawn player " + playerId);
            display.SetLives(numSpawns);
        }
    }
}
