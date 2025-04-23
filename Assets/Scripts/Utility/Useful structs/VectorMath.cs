using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct VectorMath
{
    public static Vector3 DirectionClamp(Vector3 value, float angle, Vector3 axis)
    {
        float length = value.magnitude;
        axis = axis.normalized;
        value = value.normalized;
        float cos = Vector3.Dot(value, axis);

        if (cos > Mathf.Cos(Mathf.Deg2Rad * angle)) { return value; }


        Vector3 i = Mathf.Cos(Mathf.Deg2Rad * angle) * axis;
        Vector3 j = Mathf.Sin(Mathf.Deg2Rad * angle) * (value - cos * axis).normalized;

        return (i + j) * length;
    }

    public static Vector3 DirectionClampSquare(Vector3 value, Vector2 angles, Vector3 squareNormal, Vector3 squareUp)
    {
        Vector3 squareCenterAngles = Quaternion.LookRotation(squareNormal, squareUp).eulerAngles;
        Vector3 valueAngles = Quaternion.LookRotation(value, squareUp).eulerAngles;
        Vector3 anglesDifference = valueAngles - squareCenterAngles; 

        if (Mathf.Abs(anglesDifference.x) >  angles.x)
        {
            anglesDifference.x = Mathf.Sign(anglesDifference.x) * angles.x;
        }
        if (Mathf.Abs(anglesDifference.y) > angles.y)
        {
            anglesDifference.y = Mathf.Sign(anglesDifference.x) * angles.y;
        }

        return anglesDifference + squareCenterAngles;
    }
}
