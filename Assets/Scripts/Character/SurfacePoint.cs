using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public readonly struct SurfacePoint
{
    readonly public bool IsValid;
    readonly public float Height;
    readonly public Vector3 Normal;

    private SurfacePoint(float height, Vector3 normal, bool isValid)
    {
        IsValid = isValid;
        Height = height;
        Normal = normal;
    }

    public SurfacePoint(float height, Vector3 normal)
    {
        IsValid = float.IsFinite(height) || float.IsFinite(normal.magnitude);
        Height = height;
        Normal = normal;
    }

    public static SurfacePoint NewUnchecked(float height, Vector3 normal)
    {
        return new SurfacePoint(height, normal, true);
    }

    public static SurfacePoint Invalid()
    {
        SurfacePoint sample = new(0f, Vector3.zero, false);
        return sample;  
    }
}
