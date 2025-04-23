using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct SmoothedValue
{
    [SerializeField]
    float initial;
    float current;
    [SerializeField]
    float speed;

    public readonly float Initial => initial;
    public readonly float Current => current;
    public readonly float Speed => speed;

    public SmoothedValue(float initialValue, float speed)
    {
        this.initial = initialValue; this.current = initialValue;  this.speed = speed;
    }

    public void MoveToward(float targetValue)
    {
        float newValue = current + Mathf.Sign(targetValue - current) * speed * Time.deltaTime;
        if (targetValue > current)
        {
            current = Mathf.Clamp(newValue, current, targetValue);
        }
        else
        {
            current = Mathf.Clamp(newValue, targetValue, current);
        }
    }
}
