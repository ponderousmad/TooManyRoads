using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {
	public GameObject playerPrefab;
	public int playerId;

	private bool spawn = false;
	private PlayerSettings settings;

    public Camera camera;
    private GameObject player;

	// Use this for initialization
	void Awake () {
		string playerConfig = PlayerPrefs.GetString ("Player" + playerId, "");
		if (playerConfig.CompareTo("") != 0) {
			spawn = true;
			settings = JsonUtility.FromJson<PlayerSettings> (playerConfig);
		}
	}

	void Start () {
        if (!spawn || camera == null) {
			Destroy (this.gameObject);
			return;
		}
	}

    void Update()
    {
        if(player == null)
        {
            SpawnPlayer();
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
            if(Mathf.Abs(distanceFromCamera) < cameraHorizontalSize && distanceFromCamera > bestX)
            {
                bestSpawn = go;
                bestX = distanceFromCamera;
            }
        }

        if(bestSpawn != null)
        {
            player = Instantiate(playerPrefab, bestSpawn.transform.position, Quaternion.identity);
            Destroy(bestSpawn);
            //MeshRenderer playerRenderer = player.GetComponent<MeshRenderer> ();
            //playerRenderer.material.color = settings.tint;

            player.GetComponent<Player>().SetID(playerId);
        }
    }
}
