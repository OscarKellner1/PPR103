using System;
using System.Collections;
using UnityEngine;


public struct XZSample : IEnumerable
{
    public enum Area
    {
        Forward = 0,
        Left = 1,
        Right = 2,
        Backward = 3,
    }

    public Vector3 Forward;
    public Vector3 Left;
    public Vector3 Right;
    public Vector3 Backward;

    public XZSample(float scale, float angle = 0)
    {
        scale *= 0.5f;
        var rotation = Quaternion.Euler(0f, angle, 0f);
        Forward = rotation * Vector3.forward * scale;
        Left = rotation * Vector3.left * scale;
        Right = rotation * Vector3.right * scale;
        Backward = rotation *Vector3.back * scale;
    }

    public readonly Vector3 Get(Area area)
    {
        return area switch
        {
            Area.Forward => Forward,
            Area.Left => Left,
            Area.Right => Right,
            Area.Backward => Backward,
            _ => Vector3.zero,
        };
    }

    public void Set(Area area, Vector3 vec)
    {
        switch (area)
        {
            case Area.Forward: Forward = vec; break;
            case Area.Left: Left = vec; break;
            case Area.Right: Right = vec; break;
            case Area.Backward: Backward = vec; break;
            default: break;
        }
    }

    public static Array SampleAreas()
    {
        return Enum.GetValues(typeof(Area));
    }

    public readonly XZSampleEnum GetEnumerator()
    {
        return new XZSampleEnum(this);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}


public struct XZSampleEnum : IEnumerator
{
    private readonly XZSample sample;
    private int position;

    public XZSampleEnum(XZSample sample)
    {
        this.sample = sample;
        position = -1;
    }

    public readonly object Current => sample.Get((XZSample.Area)position);

    public bool MoveNext()
    {
        position ++;
        return position <= 4;
    }

    public void Reset()
    {
        position = -1;
    }
}