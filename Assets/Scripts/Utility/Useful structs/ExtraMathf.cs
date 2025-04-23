using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct ExtraMathf
{
    public static float Mod(float a, float b)
    {
        return a - b * Mathf.Floor(a / b);
    }
}
