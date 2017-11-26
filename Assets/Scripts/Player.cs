using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public bool useOneStickMode;
    public bool useLastMove;

	private PlayerInput mInput;
    private int mPlayerID = 0;
	private PlayerSettings settings;

    [ContextMenu ("ExportInputDefintions")]
    public void ExportInputDefinitions()
    {
		System.IO.File.WriteAllText(
			@"/Users/agnomen/Documents/workspace/Inputs.txt",
			PlayerInput.AllDefinitions()
		);
    }

    public void SetID(int id)
    {
        Debug.Log("Setting player ID to: " + mPlayerID.ToString());
        mPlayerID = id;
<<<<<<< HEAD
		mInput = new PlayerInput(mPlayerID, useOneStickMode, useLastMove);
||||||| merged common ancestors
		mInput = new PlayerInput(mPlayerID, mUseOneStickMode);
=======
		mInput = new PlayerInput(mPlayerID + 1, mUseOneStickMode);
>>>>>>> origin/master

        PhysCharacterController controller = GetComponent<PhysCharacterController>();
        controller.SetPlayerInput(mInput);

		string playerConfig = PlayerPrefs.GetString ("Player" + mPlayerID, "");
		if (playerConfig.CompareTo("") != 0) {
			settings = JsonUtility.FromJson<PlayerSettings> (playerConfig);
			TryUseSettings ();
		}

		foreach(var shooter in GetComponentsInChildren<ProjectileShooter>())
		{
			shooter.SetInput(mInput);
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
					renderer.material.SetTexture ("_MainTex", settings.pattern.texture);
				}
			}
		}
	}
}
