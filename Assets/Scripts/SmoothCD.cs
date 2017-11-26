using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SmoothCD {

    public float currentValue = 0.0f;
    public float targetValue = 0.0f;
    public float velocity = 0.0f;
    public float smoothTime = 0.1f;

    public void Update(float delta)
    {
        float omega = 2.0f / smoothTime;
        float x = omega * delta;
        float ex = 1.0f / (1.0f + x + 0.48f * x * x + 0.235f * x * x * x);
        float change = currentValue - targetValue;
        float temp = (velocity + omega * change) * delta;
        velocity = (velocity - omega * temp) * ex;
        currentValue = targetValue + (change + temp) * ex;
    }
}
