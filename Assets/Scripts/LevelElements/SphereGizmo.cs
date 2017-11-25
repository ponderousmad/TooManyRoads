using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGizmo : MonoBehaviour {

    public Color color;
    public float radius;

    void OnDrawGizmos()
    {
        // Gizmos.DrawSphere(this.tra)
        Gizmos.color = color;
        Gizmos.DrawSphere(transform.position, radius);

    }
}
