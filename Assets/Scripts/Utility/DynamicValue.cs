using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct DynamicValue
{
    public float Value { get; set; }

    [SerializeField]
    float upperBound;
    [SerializeField]
    float lowerBound;
    [SerializeField]
    float changeSpeed;

    public void Increase(float deltaTime)
    {
        Value = Mathf.Clamp(Value + changeSpeed * deltaTime, lowerBound, upperBound);
    }

    public void Decrease(float deltaTime)
    {
        Value = Mathf.Clamp(Value - changeSpeed * deltaTime, lowerBound, upperBound);
    }
}
