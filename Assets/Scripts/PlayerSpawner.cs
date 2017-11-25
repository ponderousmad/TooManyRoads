using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour {
	public GameObject playerPrefab;
	public int playerId;

	private bool spawn = false;
	private PlayerSettings settings;

	// Use this for initialization
	void Awake () {
		string playerConfig = PlayerPrefs.GetString ("Player" + playerId, "");
		if (playerConfig.CompareTo("") != 0) {
			spawn = true;
			settings = JsonUtility.FromJson<PlayerSettings> (playerConfig);
		}
	}

	void Start () {
		if (!spawn) {
			Destroy (this.gameObject);
			return;
		}

		GameObject player = Instantiate (playerPrefab, transform.position, Quaternion.identity);
		//MeshRenderer playerRenderer = player.GetComponent<MeshRenderer> ();
		//playerRenderer.material.color = settings.tint;
	}
}
