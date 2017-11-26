using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingCar : MovingPlatform {

    public TrailRenderer trail;

    public float startSpeed = 1f;
    public float endSpeed = 10f;

    public float trailStart = 0.5f;
    public float trailEnd = 0.2f;
    public float startPortalPosition = 0.8f;
    public float endPortalPosition = 0.2f;

    public Collider2D myCollider;

    void Start ()
    {
       // myCollider = this.GetComponent<Collider2D>();
    }

    // Update is called once per frame
    override protected void Update () {
        base.Update();
        if(hitEnd || currentPosition > trailStart || currentPosition < trailEnd)
        {
            trail.Clear();
        }
        movementSpeed = Mathf.Lerp(startSpeed, endSpeed, currentPosition);

        if(currentPosition > endPortalPosition || currentPosition < startPortalPosition)
        {
            myCollider.enabled = false;
        } else
        {
            myCollider.enabled = true;
        }
	}

    override protected void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Vector3 delta = endPosition.position - startPosition.position;

        Gizmos.color = Color.green;
        Gizmos.DrawLine(startPosition.position, Vector3.Lerp(startPosition.position, endPosition.position, startPortalPosition));

        Gizmos.color = Color.red;
        Gizmos.DrawLine(endPosition.position, Vector3.Lerp(startPosition.position, endPosition.position, endPortalPosition));

    }
}
