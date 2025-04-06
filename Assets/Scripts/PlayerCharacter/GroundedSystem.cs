using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedSystem : MonoBehaviour
{
    [SerializeField]
    private float groundedRaycastLength = 0.1f;
    [SerializeField]
    private LayerMask groundedRaycastMask;

    public float GroundedRaycastLength => groundedRaycastLength;
    public LayerMask GroundedRaycastMask => groundedRaycastMask;

    public bool CheckGrounded()
    {
        Ray ray = new(transform.position + transform.up * 0.1f, -transform.up);
        Debug.DrawRay(ray.origin, ray.direction, Color.cyan);
        return Physics.Raycast(ray, groundedRaycastLength + 0.1f, groundedRaycastMask);
    }
}
