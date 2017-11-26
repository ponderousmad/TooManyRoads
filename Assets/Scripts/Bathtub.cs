using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bathtub : MonoBehaviour {

	public float refillRate = 1.0f;
	public GameObject fluidProjectileType;
	public float sizeConstant = 1.0f;
	public float minDropSize = 0.1f;

	public float depth = 1.0f;
	public float slope = 1.0f;

	private float mWaterAmount;
	private float mOriginalWaterScale;
	private Vector3 mOriginalWaterPosition;

	// Use this for initialization
	void Start () {
		mWaterAmount = transform.localScale.x * transform.localScale.y * transform.localScale.z;
		mOriginalWaterScale = transform.GetChild (3).localScale.x;
		mOriginalWaterPosition = transform.GetChild (3).localPosition;
	}
	
	// Update is called once per frame
	void Update () {
		mWaterAmount += refillRate * Time.deltaTime;

		Transform bathtubStart = transform.GetChild(0);
		Transform bathtubEnd = transform.GetChild(1);
		float bathtubTop = bathtubStart.position.y;

		float volume = transform.localScale.x * transform.localScale.y * transform.localScale.z;
		float excess = mWaterAmount - volume;

		if (excess < 0) {
			float waterHeight = CalcWaterSurfaceLevel (-excess);

			Transform waterTransform = transform.GetChild (3);
			waterTransform.localPosition = mOriginalWaterPosition + new Vector3 (0,-waterHeight,0);
			waterTransform.localScale = new Vector3 (mOriginalWaterScale / (1 + waterHeight * slope), waterTransform.localScale.y, waterTransform.localScale.z);
			//Debug.Log ("water height: " + waterHeight);
		}
	
		//Debug.Log ("volume: " + volume);
		//Debug.Log ("amount: " + mWaterAmount);

		if (excess >= minDropSize) {
			float variance = Random.Range (0.5f, 1.5f);
			float spillAmount = Mathf.Min(mWaterAmount, excess * variance);
//			Debug.Log ("spilling some water " + spillAmount);

			float spillX = Random.Range (bathtubStart.position.x, bathtubEnd.position.x);
			Vector3 position = new Vector3 (spillX, bathtubTop, 0.0f);
			GameObject p = Instantiate (fluidProjectileType, position, Quaternion.identity);
			KinematicProjectile k = p.GetComponent<KinematicProjectile> ();
			if (k != null) {
				k.SetInstigator (gameObject);
				k.EnableGravity (true);
			}
			Embiggener e = p.GetComponent<Embiggener> ();
			if (e != null) {
				e.SetScaleValue (spillAmount);
			}
			mWaterAmount -= spillAmount;
		}
	}

	float CalcWaterSurfaceLevel(float deficit)
	{
		float l = Mathf.Pow(deficit, 0.3f) / depth;
		return(l);
	}
}
