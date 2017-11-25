using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmbiggenNoise : MonoBehaviour {
    public Embiggener embiggenerScript;
    public float sinWave = 0.0f;
    public float waveFrequency = 4.0f;
    public float scaleNoiseLimit = 0.1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(embiggenerScript)
        {
            float scaleDiff = embiggenerScript.mTargetScaleValue - embiggenerScript.mCurrentScaleValue;

            if(Mathf.Abs(scaleDiff) > 0.001f)
            {
                sinWave += Time.deltaTime * waveFrequency;
                sinWave %= Mathf.PI;
            } else
            {
                sinWave = 0.0f;
            }

            float scaleVal = 1.0f + (Mathf.Clamp(scaleDiff, -scaleNoiseLimit, scaleNoiseLimit)) * (Mathf.Sin(sinWave) + 1.0f) * 0.5f;
            transform.localScale = new Vector3(scaleVal, scaleVal, scaleVal);
        }
	}
}
