using System;
using System.Collections;
using UnityEngine;

public struct XZSample : IEnumerable
{
    public Vector3 Center;
    public Vector3 Forward;
    public Vector3 Left;
    public Vector3 Right;
    public Vector3 Backward;

    public XZSample(float scale)
    {
        scale *= 0.5f;
        Center = Vector3.zero;
        Forward = Vector3.forward * scale;
        Left = Vector3.left * scale;
        Right = Vector3.right * scale;
        Backward = Vector3.back * scale;
    }

    public readonly Vector3 Get(SampleArea area)
    {
        return area switch
        {
            SampleArea.Center => Center,
            SampleArea.Forward => Forward,
            SampleArea.Left => Left,
            SampleArea.Right => Right,
            SampleArea.Backward => Backward,
            _ => Vector3.zero,
        };
    }

    public void Set(SampleArea area, Vector3 vec)
    {
        switch (area)
        {
            case SampleArea.Center: Center = vec; break;
            case SampleArea.Forward: Forward = vec; break;
            case SampleArea.Left: Left = vec; break;
            case SampleArea.Right: Right = vec; break;
            case SampleArea.Backward: Backward = vec; break;
            default: break;
        }
    }

    public static Array SampleAreas()
    {
        return Enum.GetValues(typeof(SampleArea));
    }

    public XZSampleEnum GetEnumerator()
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

    public readonly object Current => sample.Get((SampleArea)position);

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

public enum SampleArea
{
    Center = 0,
    Forward = 1,
    Left = 2,
    Right = 3,
    Backward = 4,
}