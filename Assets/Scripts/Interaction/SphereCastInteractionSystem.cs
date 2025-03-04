using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class SphereCastInteractionSystem : MonoBehaviour
{
    // Inspector variables
    [Header("Source")]
    [SerializeField]
    private GameObject sourceObject;

    [Header("Sphere cast")]
    [SerializeField]
    private float maxInteractionRange = 4f;
    [SerializeField]
    private float interactionSphereCastRadius = 0.3f;
    [SerializeField]
    [Description("Specifies what layers to check when sphere-casting interactible objects.")]
    private LayerMask interactionSphereCastLayers;

    [Header("Debug")]
    [SerializeField]
    private bool displayRangeIndicator = false;


    // Hidden variables
    InteractionInfo currrentInteractible;


    void Update()
    {
        Ray lookRay = new Ray(sourceObject.transform.position, sourceObject.transform.forward);
        if (Physics.SphereCast(
            lookRay,
            interactionSphereCastRadius,
            out RaycastHit hit,
            maxInteractionRange,
            interactionSphereCastLayers))
        {
            currrentInteractible = new InteractionInfo(hit.collider.GetComponent<InteractionPoint>());
        }
        else
        {
            currrentInteractible = InteractionInfo.None();
        }

        if (currrentInteractible.HasPoint())
        {
            Debug.Log("Can interact");
        }
        else
        {
            Debug.Log("Cannot interact");
        }
    }

    /// <summary>
    /// Does nothing when player is not looking at an interactible object or object is too far away.
    /// </summary>
    public bool TryInteract()
    {
        if (currrentInteractible.TryGetPoint(out InteractionPoint point))
        {
            point.Interact();
            return true;
        }
        return false;
    }


    //Editor and Gizmos
    private void Reset()
    {
        interactionSphereCastLayers = LayerMask.GetMask("Default");
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.matrix = sourceObject.transform.localToWorldMatrix;
        if (displayRangeIndicator )
        {
            Gizmos.DrawWireSphere(Vector3.zero, maxInteractionRange);
        }
    }
}
