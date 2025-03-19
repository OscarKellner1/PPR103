using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.ProBuilder;

public readonly struct SurfacePlane
{
    public readonly float ForwardHeight;
    public readonly float LeftHeight;
    public readonly float RightHeight;
    public readonly float BackwardHeight;

    public SurfacePlane(float forward, float left, float right, float backward)
    {
        ForwardHeight = forward;
        LeftHeight = left;
        RightHeight = right;
        BackwardHeight = backward;
    }

    public Vector3 Normal()
    {
        Vector3 dz = Vector3.forward + Vector3.up * (ForwardHeight - BackwardHeight);
        Vector3 dx = Vector3.right + Vector3.up * (RightHeight - LeftHeight);
        return Vector3.Cross(dz, dx).normalized;
    }
}
