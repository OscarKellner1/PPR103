using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceQuerySystem : MonoBehaviour
{
    [SerializeField]
    private float sampleSize = 1;

    public XZSample RaycastPoints { get; private set; }


    private void Start()
    {
        GenerateRaycastPoints();
    }

    public void GenerateRaycastPoints()
    {
        RaycastPoints = new XZSample(sampleSize);
    }

    public LocalSurface GetLocalSurface()
    {
        Ray ray = new(Vector3.zero, Vector3.down);

        XZSample surfacePoints = new();
        XZSample surfaceNormals = new();
        foreach (SampleArea area in XZSample.SampleAreas())
        {
            var point = RaycastPoints.Get(area);
            ray.origin = transform.TransformPoint(point);

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                surfacePoints.Set(area, transform.InverseTransformPoint(hit.point));
                surfaceNormals.Set(area, hit.normal);
                continue;
            }

            surfacePoints.Set(area, point - Vector3.up * 2f);
            surfaceNormals.Set(area, Vector3.zero);
        }

        return new LocalSurface(surfacePoints, surfaceNormals);
    }

}
