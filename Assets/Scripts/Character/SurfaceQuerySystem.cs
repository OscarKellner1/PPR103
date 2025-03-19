using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceQuerySystem : MonoBehaviour
{
    [SerializeField]
    private float _queryScale = 1;
    [SerializeField]
    private int _sampleCount = 2;

    public int SampleCount => _sampleCount;
    public XZSample[] RaycastPoints { get; private set; }



    private void Start()
    {
        GenerateRaycastPoints();
    }

    public void GenerateRaycastPoints()
    {
        RaycastPoints = new XZSample[_sampleCount];

        for (int i = 0; i < _sampleCount; i++)
        {
            RaycastPoints[i] = new XZSample(_queryScale, angle: 90f * i / _sampleCount);
        }
    }

    public LocalSurface GetSurface(int surfaceIndex)
    {
        XZSample sample = RaycastPoints[surfaceIndex];
        Ray ray = new(Vector3.zero, Vector3.down);

        XZSample surfacePoints = new();
        XZSample surfaceNormals = new();
        foreach (XZSample.Area area in XZSample.SampleAreas())
        {
            var point = sample.Get(area);
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

    public LocalSurface[] GetAllSurfaces()
    {
        LocalSurface[] surfaces = new LocalSurface[SampleCount];
        for (int i = 0; i < SampleCount; i++)
        {
            surfaces[i] = GetSurface(i);
        }
        return surfaces;
    }
}
