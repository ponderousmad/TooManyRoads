using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDisplay : MonoBehaviour {

    public GameObject[] lives = new GameObject[3];
    public Image powerBar;

    public AmmoController ammoController;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if(ammoController)
        {
            SetPowerLevel(ammoController.PowerLevel());
        }
	}

    public void SetLives(int count)
    {
        for(int i = count; i < lives.Length; ++i)
        {
            lives[i].SetActive(false);
        }
    }

    public void SetPowerLevel(float level)
    {
        float scale = Mathf.Clamp(level, 0, 1);
        powerBar.transform.localScale = new Vector3(scale, 1, 1);
    }
}
