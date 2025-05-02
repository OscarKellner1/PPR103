using System.Collections;
using UnityEngine;

public class HeldObjectHandler : MonoBehaviour
{
    public static HeldObjectHandler Instance { get; private set; }
    

    public Transform holdPoint; // Where objects appear in front of the player
    public float rayDistance = 3f;
    private SetSpotPlacer currentSnapTarget;
    public bool isLookingAtSnapSpot = false;

    public PickupableObject heldObject;
    private Renderer[] heldObjectRenderers; // Store the held object's renderers

    private Transform ghostSnapVisual;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Update()
    {
        if (heldObject != null)
        {
            TryShowSnapPosition();

            if (Input.GetKeyDown(KeyCode.F))
            {
                TryPlaceHeldObject();
            }
        }
    }

    public void HoldObject(PickupableObject obj)
    {
        heldObject = obj;
        obj.PickUp(holdPoint);

        // Store renderers to toggle visibility
        if (heldObject != null)
        {
            // Get the renderers of the held object
            heldObjectRenderers = heldObject.GetComponentsInChildren<Renderer>();
        }
    }

    private void TryShowSnapPosition()
{
    Ray cameraRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

    // Check for hit with a SetSpotPlacer object
    if (Physics.Raycast(cameraRay, out RaycastHit hit, rayDistance))
    {
        if (hit.collider.CompareTag("SetSpotPlacer"))
        {
            SetSpotPlacer spot = hit.collider.GetComponent<SetSpotPlacer>();
            if (spot != null)
            {
                    isLookingAtSnapSpot = true;
                // Hide the held object while hovering over a valid snap spot
                SetHeldObjectVisible(false);

                // Create a ghost object if it doesn't exist
                if (ghostSnapVisual == null)
                {
                    // Instantiate the held object's transform to create the ghost visual
                    ghostSnapVisual = Instantiate(heldObject.transform).transform;

                    // Set the parent of the ghost to the snap target (SetSpotPlacer)
                    ghostSnapVisual.SetParent(spot.transform);

                    // Reset the local position and rotation to match the snap position
                    ghostSnapVisual.localPosition = Vector3.zero;
                    ghostSnapVisual.localRotation = Quaternion.identity;

                    // Enable the renderer of the ghost object (which was cloned)
                    SetRendererVisible(ghostSnapVisual, true);
                }

                // Update the ghost position and rotation to match the snap position
                ghostSnapVisual.position = spot.snapPosition.position;
                ghostSnapVisual.rotation = spot.snapPosition.rotation;

                currentSnapTarget = spot; // Store the snap target to apply when placing the object
                return;
            }
        }
    }

    // If we're no longer over a valid snap spot, destroy the ghost and make the held object visible again
    if (ghostSnapVisual != null)
    {
        Destroy(ghostSnapVisual.gameObject); // Remove the ghost object
        ghostSnapVisual = null;
    }

    // Make the held object visible again
    SetHeldObjectVisible(true);
        isLookingAtSnapSpot = false;    
    currentSnapTarget = null;
}

private void SetRendererVisible(Transform obj, bool visible)
{
    // Get the renderer of the object (including children)
    Renderer[] renderers = obj.GetComponentsInChildren<Renderer>();
    foreach (Renderer rend in renderers)
    {
        rend.enabled = visible; // Enable or disable the renderer
    }
}







    public void TryPlaceHeldObject()
    {
        if (heldObject == null) return;
        if(isLookingAtSnapSpot && currentSnapTarget != null)
        {
            Destroy(ghostSnapVisual.gameObject); // Remove the ghost object
            SetHeldObjectVisible(true);
            currentSnapTarget.OnObjectPlaced(heldObject);
            StartCoroutine(PlaceObject(currentSnapTarget.snapPosition.position,currentSnapTarget.snapPosition.rotation));
            
            

            return;
        }
        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3 rayDirection = Camera.main.transform.forward;

        RaycastHit hit;
        bool mainRayHit = Physics.Raycast(rayOrigin, rayDirection, out hit, 3f);

        Debug.DrawRay(rayOrigin, rayDirection * 3f, mainRayHit ? Color.green : Color.red, 1f);

        /*// Check for SetSpotPlacer first
        if (mainRayHit && hit.collider.CompareTag("SetSpotPlacer"))
        {
            SetSpotPlacer placer = hit.collider.GetComponent<SetSpotPlacer>();
            if (placer != null)
            {
                PlaceObject(placer.transform.position, placer.transform.rotation);
                return;
            }
        }*/

        // Secondary ray logic
        Vector3 secondRayOrigin = rayOrigin + rayDirection * 3f;
        Vector3 secondRayDirection = Vector3.down;

        RaycastHit secondHit;
        bool secondRayHit = Physics.Raycast(secondRayOrigin, secondRayDirection, out secondHit, 1f);

        Debug.DrawRay(secondRayOrigin, secondRayDirection * 1f, secondRayHit ? Color.green : Color.red, 1f);

        if (mainRayHit)
        {
            float angle = Vector3.Dot(hit.normal, Vector3.up);
            if (angle >= 0.7f) // adjust this threshold if needed
            {
                StartCoroutine(PlaceObject(hit.point, Quaternion.identity));
                return;
            }
        }

        else if (secondRayHit)
        {
            StartCoroutine(PlaceObject(secondHit.point, Quaternion.identity));
        }
        else
        {
            Debug.Log("Could not place object — no valid surface found.");
        }
    }


    private void SetGhostMaterialTransparent(Transform obj)
    {
        foreach (Renderer r in obj.GetComponentsInChildren<Renderer>())
        {
            foreach (Material mat in r.materials)
            {
                mat.shader = Shader.Find("Transparent/Diffuse");
                Color c = mat.color;
                c.a = 0.5f;
                mat.color = c;
            }
        }
    }

    public bool IsHolding() => heldObject != null;

    private IEnumerator PlaceObject(Vector3 position, Quaternion rotation)
    {
        heldObject.transform.SetParent(null); // Unparent so it no longer follows the player
        heldObject.transform.position = position;
        heldObject.transform.rotation = rotation;      
       yield return new WaitForSeconds(0.0f);
        Collider col = heldObject.GetComponent<Collider>();
        col.enabled = true;
        heldObject.Drop();
        heldObject = null;
     

    }

    private void SetHeldObjectVisible(bool visible)
    {
        if (heldObjectRenderers == null) return;

        // Set the visibility of all renderers in the held object
        foreach (var renderer in heldObjectRenderers)
        {
            renderer.enabled = visible;
        }
    }


}
