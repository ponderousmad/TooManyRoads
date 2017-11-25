using UnityEngine;
using System.Collections;

public class Embiggener : MonoBehaviour {

    //public Vector3 scaleFactor = new Vector3(1.0f, 1.0f, 1.0f);
    public Vector3 minScale = new Vector3(1.0f, 1.0f, 1.0f);
    public Vector3 maxScale = new Vector3(1.0f, 1.0f, 1.0f);
    public float scaleRate = 1.0f;
    public float initialScaleLerp = 0.5f;

    public float restoreRate = 0.1f;
    public float restoreDelay = 1.0f;
    private float mRestoreTimer = 0.0f;
    //private float mMinScale = 0.0f;
    //public float maxScale = 1.0f;

    public float mCurrentScaleValue;
    public float mTargetScaleValue;
    private float mScaleRate;
    private Vector3 mOriginalScale;

	// Use this for initialization
	void Start () {
        mCurrentScaleValue = initialScaleLerp;
        mTargetScaleValue = mCurrentScaleValue;
        mOriginalScale = transform.localScale;
        UpdateScale();
	}
	
	// Update is called once per frame
	void Update () {
        if(mCurrentScaleValue != mTargetScaleValue)
        {
            if(mCurrentScaleValue < mTargetScaleValue)
            {
                mCurrentScaleValue = Mathf.Min(mTargetScaleValue, mCurrentScaleValue + Time.deltaTime * mScaleRate);
            } else if(mCurrentScaleValue > mTargetScaleValue)
            {
                mCurrentScaleValue = Mathf.Max(mTargetScaleValue, mCurrentScaleValue - Time.deltaTime * mScaleRate);
            }
            UpdateScale();
        }

        if(restoreRate > 0.0f && mCurrentScaleValue != initialScaleLerp)
        {
            if(mRestoreTimer > 0.0f)
            {
                mRestoreTimer -= Time.deltaTime;
            }
            else
            {
                mTargetScaleValue = initialScaleLerp;
                mScaleRate = restoreRate;
            }
        }
        
		/*
        // for testing
        float scaleUp = Input.GetAxis("Fire1");
        if(scaleUp > 0)
        {
            Embiggen(0.1f);
        } 
        else
        {
            float scaleDown = Input.GetAxis("Fire2");
            if(scaleDown > 0)
            {
                Embiggen(-0.1f);
            }
        }
		*/
	}

    public void Embiggen(float amount)
    {
        mTargetScaleValue = Mathf.Clamp(mTargetScaleValue+amount, 0.0f, 1.0f);
        mScaleRate = scaleRate;
        mRestoreTimer = restoreDelay;
    }

    private void UpdateScale()
    {
        float t = GetScaleLerpAmount(mCurrentScaleValue);
        float newX = mOriginalScale.x * Mathf.Lerp(minScale.x, maxScale.x, t);
        float newY = mOriginalScale.y * Mathf.Lerp(minScale.y, maxScale.y, t);
        float newZ = mOriginalScale.z * Mathf.Lerp(minScale.z, maxScale.z, t);
        transform.localScale = new Vector3(newX, newY, newZ);
    }

    private float GetScaleLerpAmount(float scaleValue)
    {
        return(scaleValue);
        //float t = Mathf.Clamp(scaleValue/scaleRate + 0.5f, 0.0f, 1.0f); 
        //return(t);
    }
}
