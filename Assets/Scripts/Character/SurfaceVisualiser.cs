using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceVisualiser : MonoBehaviour
{
    [SerializeField]
    private SurfaceQuerySystem querySystem;

    private void OnDrawGizmos()
    {
        if (querySystem == null) return;
        float sphereRadius = 0.5f;

        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = Color.magenta;
        Gizmos.DrawWireSphere(Vector3.zero, sphereRadius);


        querySystem.GenerateRaycastPoints();
        LocalSurface surface = querySystem.GetLocalSurface();

        foreach (SampleArea area in XZSample.SampleAreas())
        {
            var point = surface.Points.Get(area);

            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(point, 0.1f);
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(point, surface.Normals.Get(area) * 0.25f);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawRay(Vector3.zero, surface.CenterNormalFromAverage() * sphereRadius);

    }
}
