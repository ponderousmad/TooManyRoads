using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerData {
	public PlayerData() {
		score = 0;
		remainingLives = 3;
		playerId = -1;
	}

	public int score;
	public int remainingLives;
	public int playerId;
}

public class GameGod : MonoBehaviour {
	public string gameScene;
	public string noblelestSpiritScene;
	public float nextGameTime = 3.0f;

	private int winnerId = 0;
	private int MAX_SCORE = 3;
	private bool needsToSetWinner = false;
	private bool needsToTrackPlayers = false;
	private PlayerData [] trackers;
	private int frameDelay = 2;
	private int currentDelay = 0;
	private bool useOnePlayerHack = false;
	private float timeToNextScene = -1.0f;
	private bool firstRun = true;

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (gameObject);
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void Update () {
		if (needsToSetWinner) {
			TrySetWinnerInfo ();
		}

		if (needsToTrackPlayers) {
			TrackPlayers ();
		}

		if (timeToNextScene > 0.0f) {
			timeToNextScene -= Time.deltaTime;
			if (timeToNextScene <= 0.0f) {
				SceneManager.LoadScene (gameScene);
			}
		}
	}
	
	void OnSceneLoaded (Scene scene, LoadSceneMode mode) {
		if (scene.name.CompareTo(gameScene) == 0) {
			needsToTrackPlayers = true;
			currentDelay = 0;
		} else if (scene.name.CompareTo(noblelestSpiritScene) == 0) {
			needsToSetWinner = true;
		}
	}

	void TrackPlayers () {
		if (currentDelay < frameDelay) {
			currentDelay++;
			return;
		}

		Player[] players = FindObjectsOfType<Player> ();

		if (firstRun) {
			trackers = new PlayerData[players.Length];
		} else {
			for (int i = 0; i < trackers.Length; ++i) {
				trackers [i].remainingLives = 3;
			}
		}

		for (int i = 0; i < players.Length; ++i) {
			players [i].SetGod (this);

			if (firstRun) {
				trackers [i] = new PlayerData ();
				trackers [i].playerId = players [i].GetId ();
			}
		}

		if (trackers.Length == 1) {
			useOnePlayerHack = true;
		}

		firstRun = false;
		needsToTrackPlayers = false;
	}

	void TrySetWinnerInfo () {
		Player player = FindObjectOfType<Player> ();
		if (player) {
			player.SetGod (this);
			player.SetID (winnerId);
			needsToSetWinner = false;
			SceneManager.sceneLoaded -= OnSceneLoaded;
			Destroy (gameObject);
		}
	}

	public void OnPlayerDied (int playerId) {
		for (int i = 0; i < trackers.Length; ++i) {
			if (trackers [i].playerId == playerId) {
				trackers [i].remainingLives--;
			}			
		}

		int deadCount = 0;
		for (int i = 0; i < trackers.Length; ++i) {
			if (trackers [i].remainingLives == 0) {
				deadCount++;
			}
		}

		if (useOnePlayerHack) {
			if (trackers[0].remainingLives == 0) {
				winnerId = 0;
				SceneManager.LoadSceneAsync (noblelestSpiritScene);
			}
		} else {
			if (deadCount >= (trackers.Length - 1)) {
				int aliveId = -1;
				for (int i = 0; i < trackers.Length; ++i) {
					if (trackers [i].remainingLives > 0) {
						aliveId = trackers[i].playerId;
						break;
					}
				}

				trackers [aliveId].score++;
				if (trackers [aliveId].score == MAX_SCORE) {
					winnerId = aliveId;
					SceneManager.LoadSceneAsync (noblelestSpiritScene);
				} else {
					timeToNextScene = nextGameTime;
				}
			}
		}
	}
}
