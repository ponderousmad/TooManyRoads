using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CounterScale : MonoBehaviour {

    public GameObject parent;

	void Update () {
        this.transform.localScale = new Vector3(1f / parent.transform.localScale.x, 1f/parent.transform.localScale.y, 1f/parent.transform.localScale.z);
	}
}
