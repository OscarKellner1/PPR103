using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtraGizmos
{
    public static void DrawArrow(Vector3 start, Vector3 direction)
    {
        var arrowDirection = Quaternion.FromToRotation(Vector3.forward, direction);

        Vector3 arrowStart = start;
        Vector3 arrowTip = arrowStart + direction;
        Gizmos.DrawLine(arrowStart, arrowTip);

        Gizmos.DrawLine(arrowTip, arrowTip - arrowDirection * (Vector3.forward + Vector3.up) * 0.3f);
        Gizmos.DrawLine(arrowTip, arrowTip - arrowDirection * (Vector3.forward - Vector3.up) * 0.3f);
        Gizmos.DrawLine(arrowTip, arrowTip - arrowDirection * (Vector3.forward + Vector3.right) * 0.3f);
        Gizmos.DrawLine(arrowTip, arrowTip - arrowDirection * (Vector3.forward - Vector3.right) * 0.3f);
    }

    public static void DrawArrow(Vector3 start, Vector3 direction, float length)
    {
        var arrowDirection = Quaternion.FromToRotation(Vector3.forward, direction);

        Vector3 arrowTip = start + direction * length;
        Gizmos.DrawLine(start, arrowTip);

        Gizmos.DrawLine(arrowTip, arrowTip - arrowDirection * (Vector3.forward + Vector3.up) * 0.3f);
        Gizmos.DrawLine(arrowTip, arrowTip - arrowDirection * (Vector3.forward - Vector3.up) * 0.3f);
        Gizmos.DrawLine(arrowTip, arrowTip - arrowDirection * (Vector3.forward + Vector3.right) * 0.3f);
        Gizmos.DrawLine(arrowTip, arrowTip - arrowDirection * (Vector3.forward - Vector3.right) * 0.3f);
    }
}
