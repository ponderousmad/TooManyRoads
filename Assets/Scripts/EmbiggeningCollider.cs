using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmbiggeningCollider : CollisionResponse {

	public float embiggenAmount = 0.1f;
    public AudioClip sound;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public override void Collision(Collider2D other)
	{
		Embiggener embiggener = other.GetComponent<Embiggener>();
		if(embiggener != null) 
		{
			if(embiggener.Embiggen(embiggenAmount))
            {
                Debug.Log("Embigged: " + embiggenAmount);
                if(sound)
                {
                    Debug.Log("Played sound");
                    AudioSource.PlayClipAtPoint(sound, transform.position, 1.0f);
                }
            }
		}
	}
}
