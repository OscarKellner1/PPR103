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

    private float sphereCastRadius;

    public float GroundedSpherecastLength => groundedSpherecastLength;
    public LayerMask GroundedSpherecastMask => groundedSpherecastMask;
    public UnityEvent<RaycastHit?> OnCheckGround { get; private set; } = new UnityEvent<RaycastHit?>();

    private void Start()
    {
        sphereCastRadius = GetComponent<CapsuleCollider>().radius;
    }

    /// <summary>
    /// Should only be called by character controller to control the correct executino order.
    /// </summary>
    /// <returns></returns>
    public RaycastHit? CheckGrounded()
    {
        Ray ray = new(transform.position + transform.up * (sphereCastRadius + 0.1f), -transform.up);
        Debug.DrawRay(ray.origin, ray.direction);
        if (Physics.SphereCast(ray,
                               sphereCastRadius,
                               out RaycastHit raycastHit,
                               groundedSpherecastLength + 0.1f,
                               groundedSpherecastMask,
                               QueryTriggerInteraction.Ignore))
        {
            OnCheckGround.Invoke(raycastHit);
            return raycastHit;
        }
        else
        {
            OnCheckGround.Invoke(null);
            return null;
        }

    }
}
