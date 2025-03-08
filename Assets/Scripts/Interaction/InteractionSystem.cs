using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

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

    [Header("UI")]
    [SerializeField]
    private GameObject uiPrefab;

    [Header("Debug")]
    [SerializeField]
    private bool displayRangeIndicator = false;


    // Hidden variables
    InteractionQuery interactionInfo;
    GameObject UIElement;


    private void Start()
    {
        UIElement = UISystem.AddElement(uiPrefab);
        UIElement.SetActive(false);
    }

    void Update()
    {
        Ray lookRay = new(sphereCastSource.transform.position, sphereCastSource.transform.forward);
        if (Physics.SphereCast(
            lookRay,
            interactionSphereCastRadius,
            out RaycastHit hit,
            maxInteractionRange,
            interactionSphereCastLayers))
        {
            interactionInfo = new InteractionQuery(hit.collider.GetComponent<InteractionObject>());
        }
        else
        {
            interactionInfo = InteractionQuery.None();
        }

        if (interactionInfo.HasObject())
        {
            UIElement.SetActive(true);
        }
        else
        {
            UIElement.SetActive(false);
        }
    }

    /// <summary>
    /// Does nothing when player is not looking at an interactible object or object is too far away.
    /// </summary>
    public bool TryInteract()
    {
        if (interactionInfo.TryGetObject(out InteractionObject interactionObject))
        {
            interactionObject.Interact();
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
        if (displayRangeIndicator )
        {
            Gizmos.color = Color.green;
            Gizmos.matrix = sphereCastSource.transform.localToWorldMatrix;
            Gizmos.DrawWireSphere(Vector3.zero, maxInteractionRange);
        }
    }
}
