using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizerNavigator : MonoBehaviour {
	public int playerId;
	public PlayerChooser chooserMgr;

	private PlayerInfo playerRef;

	void Start () {
		chooserMgr.GetPlayerInfo (playerId, ref playerRef);
	}

	// Update is called once per frame
	void Update () {
		if (playerRef.isActive) {
			
		}
	}
}
