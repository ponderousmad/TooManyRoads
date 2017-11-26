using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {
	public bool mUseOneStickMode;

	private PlayerInput mInput;
    private int mPlayerID = 0;

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
		mInput = new PlayerInput(mPlayerID, mUseOneStickMode);

        PhysCharacterController controller = GetComponent<PhysCharacterController>();
        controller.SetPlayerInput(mInput);
    }

	// Use this for initialization
	void Start () {
        if(mPlayerID == 0)
        {
            Debug.Log("Using default player ID.");
            SetID(1);
        }

        foreach(var shooter in GetComponentsInChildren<ProjectileShooter>())
        {
            shooter.SetInput(mInput);
        }
	}
	
	// Update is called once per frame
	void Update () {
        bool debugInput = false;
        if(debugInput)
        {
            mInput.Aim(false);
            Debug.Log("Move: " + mInput.MoveX.ToString() + ", " + mInput.MoveY.ToString());
            Debug.Log("Fire Embiggen: " + mInput.FireEmbiggen.ToString());
            Debug.Log("Fire Debigulate: " + mInput.FireDebigulate.ToString());
        }
	}

	public PlayerInput Input { get { return mInput; } }
}
