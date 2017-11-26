using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageController : MonoBehaviour {

	public float health;
	public AudioClip hurtSound;
	public AudioClip deathSound;
    public GameObject deathEffect;

	private float mCurrentHealth;
	private bool mDead;
	private AudioSource mAudioSource;
	private Player mPlayerRef;

	// Use this for initialization
	void Start () {
		mCurrentHealth = health;
		mDead = false;
	}

	public void Damage(float damageAmount)
	{
		if (mDead) {
			return;
		}

		mCurrentHealth -= damageAmount;
		if (mCurrentHealth <= 0) {
			if (deathSound != null) {
				mAudioSource.PlayOneShot (deathSound);
			}
			Die ();
		} else {
			if (hurtSound != null) {
				mAudioSource.PlayOneShot (hurtSound);
			}
		}
	}

	void Die()
	{
		mDead = true;
		mPlayerRef.SendDeath ();

        if(deathEffect)
        {
            var effect = Instantiate(deathEffect, transform.position, Quaternion.identity);
            effect.transform.localScale = transform.localScale;
        }

		//Debug.Log ("I died!");
        Destroy(this.gameObject);
	}

	public void SetPlayer (Player myPlayer) {
		mPlayerRef = myPlayer;
	}

	public void SetAudioSource(AudioSource audioSource) {
		mAudioSource = audioSource;
	}
}
