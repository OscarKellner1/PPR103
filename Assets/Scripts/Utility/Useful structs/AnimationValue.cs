using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct AnimationValue
{
    public float Value { get; set; }

    [SerializeField]
    float speed;

    public void Increase(float deltaTime)
    {
        Value = Mathf.Clamp(Value + speed * deltaTime, 0, 1);
    }

    public void Decrease(float deltaTime)
    {
        Value = Mathf.Clamp(Value - speed * deltaTime, 0, 1);
    }
}
