﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public bool useOneStickMode;
    public bool useLastMove;

	public Texture2D fallbackTexture;

	private PlayerInput mInput;
    private int mPlayerID = 0;
	private PlayerSettings settings;
	private bool wentToChooseLevel;

	private AudioSource audioSource;
	public AudioClip jump;
	public AudioClip shootEmbig;
	public AudioClip shootDebig;
	public AudioClip die;

    [ContextMenu ("ExportInputDefintions")]
    public void ExportInputDefinitions()
    {
		System.IO.File.WriteAllText(
			@"/Users/agnomen/Documents/workspace/git/TooManyRoads/ProjectSettings/InputManager.asset",
			PlayerInput.AllDefinitions()
		);
    }

    public void SetID(int id)
    {
		if (GameObject.FindGameObjectWithTag("Fallback")) {
			wentToChooseLevel = true;
		}

        Debug.Log("Setting player ID to: " + mPlayerID.ToString());
		audioSource = GetComponent<AudioSource> ();

        mPlayerID = id;
		mInput = new PlayerInput(mPlayerID + 1, useOneStickMode, useLastMove);

        PhysCharacterController controller = GetComponent<PhysCharacterController>();
        controller.SetPlayerInput(mInput);
		controller.SetAudioSource (audioSource);

		foreach(var shooter in GetComponentsInChildren<ProjectileShooter>())
		{
			shooter.SetInput(mInput);
			shooter.SetAudioSource (audioSource);
		}

		DamageController dm = GetComponent<DamageController> ();
		dm.SetAudioSource (audioSource);

		string playerConfig = PlayerPrefs.GetString ("Player" + mPlayerID, "");
		if (playerConfig.CompareTo("") != 0) {
			settings = JsonUtility.FromJson<PlayerSettings> (playerConfig);
			TryUseSettings ();
		}
    }
	
	// Update is called once per frame
	void Update () {
        bool debugInput = false;
		if(debugInput && mInput != null)
        {
            mInput.Aim(false);
            Debug.Log("Move: " + mInput.MoveX.ToString() + ", " + mInput.MoveY.ToString());
            Debug.Log("Fire Embiggen: " + mInput.FireEmbiggen.ToString());
            Debug.Log("Fire Debigulate: " + mInput.FireDebigulate.ToString());
        }
	}

	public PlayerInput Input { get { return mInput; } }

	void TryUseSettings() {
		for (int i = 0; i < transform.childCount; ++i) {
			Transform child = transform.GetChild (i);
			if (child.gameObject && child.gameObject.CompareTag ("Visuals")) {
				MeshRenderer renderer = child.GetComponent<MeshRenderer> ();
				if (renderer) {
					renderer.material.SetColor ("_Color", settings.tint);
					renderer.material.SetColor ("_Accent", settings.stripe);

					if (wentToChooseLevel && settings.pattern != null && settings.pattern.texture != null) {
						renderer.material.SetTexture ("_MainTex", settings.pattern.texture);
					} else {
						renderer.material.SetTexture ("_MainTex", fallbackTexture);
					}
				}
			}
		}
	}
}
