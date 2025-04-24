using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct RepeatingEventTimeline
{
    [SerializeField]
    float value;
    [SerializeField]
    float eventActivationThreshold;
    public event Action Event;

    public void TraverseTimeline(float speed)
    {
        float newValue = value + speed * Time.deltaTime;
        if (newValue > eventActivationThreshold)
        {
            value = ExtraMathf.Mod(newValue, eventActivationThreshold);
            Event.Invoke();
        }
        else
        {
            value = newValue;
        }
    }
}
