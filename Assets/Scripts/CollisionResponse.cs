using UnityEngine;

using System;

public abstract class CollisionResponse : MonoBehaviour
{
	public CollisionResponse ()
	{
	}

	public abstract void Collision(Collider2D other);
}

