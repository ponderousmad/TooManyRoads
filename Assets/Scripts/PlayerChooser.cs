using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfo {
	public PlayerInfo () {
		settings = new PlayerSettings ();
	}

	public GameObject activeUI;
	public GameObject chooseUI;
	public bool isActive;
	public PlayerSettings settings;
}

public class PlayerChooser : MonoBehaviour {
	const int MAX_PLAYERS = 4;

	public string mainSceneName;
	public int minPlayers = 1;
	public GameObject activeUIPrefab;
	public GameObject chooseUIPrefab;
	public Transform[] uiAnchors;

	private PlayerInfo[] players;
	private int currentPlayers = 0;

	// Use this for initialization
	void Start () {
		players = new PlayerInfo[MAX_PLAYERS];

		for (int i = 0; i < MAX_PLAYERS; ++i) {
			players [i] = new PlayerInfo ();
			players [i].chooseUI = Instantiate (chooseUIPrefab, uiAnchors [i].position, Quaternion.identity);
			players [i].activeUI = Instantiate (activeUIPrefab, uiAnchors [i].position, Quaternion.identity);
			players [i].activeUI.SetActive (false);
			players [i].isActive = false;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown ("Jump")) {
			if (!players [0].isActive) {
				ShowPlayer (0);
			}
		} else if (Input.GetButtonDown ("Fire1")) {
			if (players [0].isActive) {
				HidePlayer (0);
			}
		}

		if (Input.GetButtonDown ("Fire2")) {
			if (currentPlayers >= minPlayers) {
				StartGame ();
			}
		}
	}

	void ShowPlayer (int id) {
		players [id].isActive = true;
		players [id].activeUI.SetActive (true);
		players [id].chooseUI.SetActive (false);
		++currentPlayers;
	}

	void HidePlayer (int id) {
		players [id].isActive = false;
		players [id].activeUI.SetActive (false);
		players [id].chooseUI.SetActive (true);
		--currentPlayers;
	}

	void StartGame () {
		for (int i = 0; i < MAX_PLAYERS; ++i) {
			if (players [i].isActive) {
				PlayerPrefs.SetString ("Player" + i, JsonUtility.ToJson (players [i].settings));
			} else {
				PlayerPrefs.SetString ("Player" + i, "");
			}
		}
		SceneManager.LoadSceneAsync (mainSceneName);
	}
}
