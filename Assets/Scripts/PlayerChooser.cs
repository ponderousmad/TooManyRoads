using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerInfo {
	public PlayerInfo (int id) {
		settings = new PlayerSettings ();
		input = new PlayerInput (id + 1);
	}

	public GameObject activeUI;
	public GameObject chooseUI;
	public bool isActive;
	public PlayerSettings settings;
	public PlayerInput input;
}

public class PlayerChooser : MonoBehaviour {
	const int MAX_PLAYERS = 4;

	public string mainSceneName;
	public int minPlayers = 2;
	public GameObject[] activeUIs;
	public GameObject[] chooseUIs;

	private PlayerInfo[] players;
	private int currentPlayers = 0;

	// Use this for initialization
	void Awake () {
		players = new PlayerInfo[MAX_PLAYERS];

		for (int i = 0; i < MAX_PLAYERS; ++i) {
			players [i] = new PlayerInfo (i);
			players [i].chooseUI = chooseUIs[i];
			players [i].activeUI = activeUIs[i];
			players [i].activeUI.SetActive (false);
			players [i].isActive = false;
		}
	}

	// Update is called once per frame
	void Update () {
		InputCheck ();
	}

	void InputCheck () {
		for (int i = 0; i < MAX_PLAYERS; ++i) {
			// Join or leave check
			if (players [i].input.FireEmbiggen) {
				if (!players [i].isActive) {
					ShowPlayer (i);
				}
			} else if (players[i].input.FireDebigulate) {
				if (players [i].isActive) {
					HidePlayer (i);
				}
			}

			// Start the game check
			if (players[i].input.MenuPress && currentPlayers >= minPlayers) {
				StartGame ();
			}
		}
	}

	void ShowPlayer (int id) {
		players [id].isActive = true;
		players [id].activeUI.SetActive (true);
		players [id].chooseUI.SetActive (false);
        Debug.Log("Showing Player " + id);
		++currentPlayers;
	}

	void HidePlayer (int id) {
		players [id].isActive = false;
		players [id].activeUI.SetActive (false);
		players [id].chooseUI.SetActive (true);
        Debug.Log("Hiding Player " + id);
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

	public PlayerInfo GetPlayerInfo(int id) {
		return(players [id]);
	}
}
