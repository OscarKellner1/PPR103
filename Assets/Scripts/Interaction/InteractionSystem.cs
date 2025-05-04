using System.ComponentModel;
using UnityEngine;
using UnityEngine.Events;

public class InteractionSystem : MonoBehaviour
{
    // Inspector variables
    [Header("Sphere cast")]
    [SerializeField]
    private GameObject sphereCastSource;
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

    public UnityEvent<InteractionObject> OnLookAtChange { get; private set; } = new();
    public InteractionObject LookAtObject => interactionObject;

    // Hidden variables
    InteractionObject interactionObject;


    void Update()
    {
        InteractionObject lookAtObject = GetLookAtObject();

        if (interactionObject != lookAtObject)
        {
            interactionObject = lookAtObject;
            OnLookAtChange?.Invoke(interactionObject);
        }
    }

    /// <summary>
    /// Does nothing when player is not looking at an interactible object or object is too far away.
    /// </summary>
    public bool TryInteract()
    {
        if (interactionObject != null)
        {
            interactionObject.Interact();
            return true;
        }

        return false;
    }

    private InteractionObject GetLookAtObject()
    {
        Ray lookRay = new(sphereCastSource.transform.position, sphereCastSource.transform.forward);
        if (Physics.SphereCast(
            lookRay,
            interactionSphereCastRadius,
            out RaycastHit hit,
            maxInteractionRange,
            interactionSphereCastLayers))
        {
            return hit.collider.GetComponent<InteractionObject>();
        }
        else
        {
            return null;
        }
    }

    //Editor and Gizmos
    private void Reset()
    {
        interactionSphereCastLayers = LayerMask.GetMask("Default");
    }

    private void OnDrawGizmos()
    {
        if (displayRangeIndicator )
        {
            Gizmos.color = Color.green;
            Gizmos.matrix = sphereCastSource.transform.localToWorldMatrix;
            Gizmos.DrawWireSphere(Vector3.zero, maxInteractionRange);
        }
    }
}
