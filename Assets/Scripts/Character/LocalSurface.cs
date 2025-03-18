using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public readonly struct LocalSurface
{
    public readonly XZSample Points;
    public readonly XZSample Normals;

    public LocalSurface(XZSample points, XZSample normals)
    {
        this.Points = points;
        this.Normals = normals;
    }

    public Vector3 CenterNormal()
    {
        return Normals.Center;
    }

    public Vector3 CenterNormalFromAverage()
    {
        Vector3 dz = (Points.Forward - Points.Backward).normalized;
        Vector3 dx = (Points.Right - Points.Left).normalized;

        return Vector3.Cross(dz, dx).normalized;
    }
}