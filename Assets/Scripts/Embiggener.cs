using UnityEngine;
using System.Collections;

public class Embiggener : MonoBehaviour {

    //public Vector3 scaleFactor = new Vector3(1.0f, 1.0f, 1.0f);
    public Vector3 minScale = new Vector3(1.0f, 1.0f, 1.0f);
    public Vector3 maxScale = new Vector3(1.0f, 1.0f, 1.0f);
    public float scaleRate = 1.0f;
    public float initialScaleLerp = 0.5f;
	public float lerpScaling = 1.0f;

    public float restoreRate = 0.1f;
    public float restoreDelay = 1.0f;
    
	public bool scalingAffectsMass = true;

	internal float mCurrentScaleValue;
    internal float mTargetScaleValue;

	private float mRestoreTimer = 0.0f;
    private float mScaleRate;
    private Vector3 mOriginalScale;
	private float mOriginalMass;
	private float mOriginalVolume;

	private Rigidbody2D mRigidbody;

	void Awake()
	{
		mCurrentScaleValue = initialScaleLerp;
		mTargetScaleValue = mCurrentScaleValue;
		mOriginalScale = transform.localScale;
		mOriginalVolume = mOriginalScale.x * mOriginalScale.y * mOriginalScale.z;

		mRigidbody = GetComponent<Rigidbody2D> ();
		if (mRigidbody != null) {
			mOriginalMass = mRigidbody.mass;
		}

		UpdateScale();
	}

	// Use this for initialization
	void Start () {

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
                float targetScaleDiff = initialScaleLerp - mCurrentScaleValue;
                mTargetScaleValue += Mathf.Min(Mathf.Abs(targetScaleDiff), restoreRate * Time.deltaTime) * Mathf.Sign(targetScaleDiff);
            }
        }

	}

    public void Embiggen(float amount)
    {
		mTargetScaleValue = Mathf.Clamp(mTargetScaleValue+amount*lerpScaling, 0.0f, 1.0f);
        mScaleRate = scaleRate;
        mRestoreTimer = restoreDelay;
    }

	public void SetScaleValue(float value)
	{
		mCurrentScaleValue = Mathf.Clamp (value, 0.0f, 1.0f);
		mTargetScaleValue = mCurrentScaleValue;
		UpdateScale ();
	}

    private void UpdateScale()
    {
        float t = GetScaleLerpAmount(mCurrentScaleValue);
        float newX = mOriginalScale.x * Mathf.Lerp(minScale.x, maxScale.x, t);
        float newY = mOriginalScale.y * Mathf.Lerp(minScale.y, maxScale.y, t);
        float newZ = mOriginalScale.z * Mathf.Lerp(minScale.z, maxScale.z, t);
        transform.localScale = new Vector3(newX, newY, newZ);

		if (scalingAffectsMass && mRigidbody != null) {
			float roughVolume = transform.localScale.x * transform.localScale.y * transform.localScale.z;
			mRigidbody.mass = mOriginalMass * (roughVolume / mOriginalVolume);
		}
    }

    private float GetScaleLerpAmount(float scaleValue)
    {
		return(scaleValue);
        //float t = Mathf.Clamp(scaleValue/scaleRate + 0.5f, 0.0f, 1.0f); 
        //return(t);
    }
}
