using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject playerCam;
    [SerializeField]
    private float maxGrabDistance = 2f;

    public bool grabbingRight { get; private set; }
    private Vector3 rightGrabPos;

    public bool grabbingLeft {get; private set;}
    private Vector3 leftGrabPos;

    public bool TryGrabRight(out Vector3 grabPos)
    {
        Ray ray = new(playerCam.transform.position, playerCam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxGrabDistance))
        {
            grabbingRight = true;
            rightGrabPos = hit.point;
            grabPos = rightGrabPos;
            return true;
        }
        grabPos = Vector3.positiveInfinity;
        return false;
    }

    public void ReleaseRight()
    {
        grabbingRight = false;
    }

    public bool TryGrabLeft(out Vector3 grabPos)
    {
        Ray ray = new(playerCam.transform.position, playerCam.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, maxGrabDistance))
        {
            grabbingLeft = true;
            leftGrabPos = hit.point;
            grabPos = leftGrabPos;
            return true;
        }
        grabPos = Vector3.positiveInfinity;
        return false;
    }

    public void ReleaseLeft()
    {
        grabbingLeft = false;
    }

    private void OnDrawGizmos()
    {
        if (grabbingRight)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(rightGrabPos, 0.2f);
        }

        if (grabbingLeft)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(leftGrabPos, 0.2f);
        }
    }
}
