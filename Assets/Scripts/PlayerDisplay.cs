using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDisplay : MonoBehaviour {

    public GameObject[] lives = new GameObject[3];
    public Image powerBar;

    private float powerLevel = 1;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        powerLevel -= Time.deltaTime;
		SetPowerLevel(powerLevel);
        if(powerLevel < 0.2)
        {
            RemoveLife(0);
        }
	}

    public void RemoveLife(int index)
    {
        lives[index].SetActive(false);
    }

    public void SetPowerLevel(float level)
    {
        float scale = Mathf.Clamp(level, 0, 1);
        powerBar.transform.localScale = new Vector3(scale, 1, 1);
    }
}
