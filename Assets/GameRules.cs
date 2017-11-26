using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRules : MonoBehaviour {

    public delegate void GameOver();
    public static event GameOver OnGameOver;

    int playerCount = 0;
    int deadPlayers = 0;

    bool gameIsOver = false;
    float timeToNextLevel = 5.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnPlayerSpawned(PlayerSpawner player)
    {
        ++playerCount;
    }

    public void OnPlayerEliminated(PlayerSpawner player)
    {
        ++deadPlayers;

        if(playerCount == 1)
        {
            print("GAME OVER! PLEASE DEPOSIT 40 QUARTERS!");
            gameIsOver = true;
            OnGameOver();
        } else if(deadPlayers == playerCount - 1)
        {
            print("We have a winner!");
            gameIsOver = true;
            OnGameOver();
        }
    }
}
