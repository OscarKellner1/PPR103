using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundedSystem : MonoBehaviour
{
    [SerializeField]
    private float groundedSpherecastLength = 0.1f;
    [SerializeField]
    private LayerMask groundedSpherecastMask;
    [SerializeField]
    private float sphereCastRadius;

    public float GroundedSpherecastLength => groundedSpherecastLength;
    public LayerMask GroundedSpherecastMask => groundedSpherecastMask;

    private void Start()
    {
        sphereCastRadius = GetComponent<CapsuleCollider>().radius;
    }

    public GroundCheckResult CheckGrounded()
    {
        Ray ray = new(transform.position + transform.up * (sphereCastRadius + 0.1f), -transform.up);
        Debug.DrawRay(ray.origin, ray.direction);
        if (Physics.SphereCast(ray, sphereCastRadius, out RaycastHit raycastHit, groundedSpherecastLength + 0.1f, groundedSpherecastMask))
        {
            return GroundCheckResult.Hit(raycastHit.normal);
        }
        else
        {
            return GroundCheckResult.NoHit();
        }
    }
}

public readonly struct GroundCheckResult
{
    public readonly bool hit;
    public readonly Vector3 normal;

    private GroundCheckResult(bool hit, Vector3 normal)
    {
        this.hit = hit;
        this.normal = normal;
    }

    public static GroundCheckResult Hit(Vector3 normal)
    {
        return new GroundCheckResult(true, normal);
    }

    public static GroundCheckResult NoHit()
    {
        return new GroundCheckResult(false, Vector3.zero);
    }
}
