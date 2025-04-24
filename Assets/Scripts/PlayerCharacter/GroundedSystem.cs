using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
    public UnityEvent<GroundCheckResult> OnCheckGround { get; private set; }

    private void Start()
    {
        sphereCastRadius = GetComponent<CapsuleCollider>().radius;
    }

    /// <summary>
    /// Should only be called by character controller to control the correct executino order.
    /// </summary>
    /// <returns></returns>
    public GroundCheckResult CheckGrounded()
    {
        Ray ray = new(transform.position + transform.up * (sphereCastRadius + 0.1f), -transform.up);
        Debug.DrawRay(ray.origin, ray.direction);
        if (Physics.SphereCast(ray, sphereCastRadius, out RaycastHit raycastHit, groundedSpherecastLength + 0.1f, groundedSpherecastMask))
        {
            return GroundCheckResult.Hit(raycastHit.normal, raycastHit.triangleIndex);
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
    public readonly int triangleIndex;

    private GroundCheckResult(bool hit, Vector3 normal, int triangleIndex)
    {
        this.hit = hit;
        this.normal = normal;
        this.triangleIndex = triangleIndex;
    }

    public static GroundCheckResult Hit(Vector3 normal, int triangleIndex)
    {
        return new GroundCheckResult(true, normal, triangleIndex);
    }

    public static GroundCheckResult NoHit()
    {
        return new GroundCheckResult(false, Vector3.zero, 0);
    }
}
