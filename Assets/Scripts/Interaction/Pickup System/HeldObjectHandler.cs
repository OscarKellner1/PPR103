using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeldObjectHandler : MonoBehaviour
{
    public static HeldObjectHandler Instance { get; private set; }

    [System.Serializable]
    public struct HoldPointEntry
    {
        public string objectName;   // must exactly match PickupableObject.ObjectName
        public Transform holdPoint; // the custom hold-point for that object
    }

    [Header("Per-Object Hold Points")]
    public List<HoldPointEntry> holdPointEntries;
    private Dictionary<string, Transform> _holdPointMap;

    // fallback if no match is found:
    [Tooltip("Used if no matching entry in holdPointEntries")]
    public Transform defaultHoldPoint;

    //public Transform holdPoint; // Where objects appear in front of the player
    public float rayDistance = 3f;
    private SetSpotPlacer currentSnapTarget;
    public bool isLookingAtSnapSpot = false;

    public PickupableObject heldObject;
    private Renderer[] heldObjectRenderers; // Store the held object's renderers

    private Transform ghostSnapVisual;
    [Space(10)]
    public Material PreviewMaterial;

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
        _holdPointMap = new Dictionary<string, Transform>(holdPointEntries.Count);
        foreach (var entry in holdPointEntries)
        {
            if (entry.objectName != null && entry.holdPoint != null)
                _holdPointMap[entry.objectName] = entry.holdPoint;
        }

    }

    void Update()
    {
        if (heldObject != null)
        {
            if (IsLookingTooFarDown(heldObject.MaxDwnAngle))
            {
                if (ghostSnapVisual != null)
                {
                    Destroy(ghostSnapVisual.gameObject); // Remove the ghost object
                    ghostSnapVisual = null;
                }

                // Make the held object visible again
                SetHeldObjectVisible(true);
                isLookingAtSnapSpot = false;
                currentSnapTarget = null;
                return;
            }
            TryShowSnapPosition();

            if (Input.GetKeyDown(KeyCode.F))
            {
                TryPlaceHeldObject();
            }
        }
    }

    /// <summary>
    /// Returns the custom hold‐point for a given object name,
    /// or the default if none was registered.
    /// </summary>
    public Transform GetHoldPointFor(string objectName)
    {
        if (_holdPointMap.TryGetValue(objectName, out var t))
            return t;
        return defaultHoldPoint;
    }


    public void HoldObject(PickupableObject obj)
    {
        heldObject = obj;

        // pick the right holdPoint:
        Transform target;
        if (!_holdPointMap.TryGetValue(obj.ObjectName, out target))
        {
            Debug.Log("No Dedicated Hold Point For That Object");
            target = defaultHoldPoint;


        }


        obj.PickUp(target);

        // store renderers for visibility toggling
        heldObjectRenderers = heldObject.GetComponentsInChildren<Renderer>();
    }

    private void TryShowSnapPosition()
{
    Ray cameraRay = new Ray(Camera.main.transform.position, Camera.main.transform.forward);

    // Check for hit with a SetSpotPlacer object
    if (Physics.Raycast(cameraRay, out RaycastHit hit, rayDistance))
    {
        if (hit.collider.CompareTag("SetSpotPlacer"))
        {
                if (IsLookingTooFarDown(heldObject.MaxDwnAngle)) return;
                SetSpotPlacer spot = hit.collider.GetComponent<SetSpotPlacer>();
                if (spot.CorrectName != heldObject.ObjectName) return;
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
                        MakeCloneTheColor(ghostSnapVisual);
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






    public GameObject CameraController;
    

    bool IsLookingTooFarDown(float angle)
    {
        // read the camera’s X-euler (0→360)
        float pitch = Camera.main.transform.eulerAngles.x;
        // convert to –180…+180
        if (pitch > 180f) pitch -= 360f;
        // now pitch > 0 means looking down, < 0 means looking up
    
        return pitch > angle;
    }

    public void TryPlaceHeldObject()
    {
        
    
        if (heldObject == null) return;

        // === SNAP-SPOT LOGIC ===
        if (isLookingAtSnapSpot && currentSnapTarget != null)
        {
            // 1) Destroy the ghost preview if there is one
            if (ghostSnapVisual != null)
            {
                Destroy(ghostSnapVisual.gameObject);
                ghostSnapVisual = null;
            }

            // 2) Re-show the real held object
            SetHeldObjectVisible(true);
            // (just in case, also individually re-enable its renderers)
            foreach (var rend in heldObjectRenderers)
                rend.enabled = true;

            // 3) Parent and snap the held object into place
            heldObject.transform.SetParent(currentSnapTarget.transform, worldPositionStays: false);
            heldObject.transform.localPosition = currentSnapTarget.snapPosition.localPosition;
            heldObject.transform.localRotation = currentSnapTarget.snapPosition.localRotation;

            // 4) Notify the spot and finalize
            currentSnapTarget.GotSomething = true;
            currentSnapTarget.OnObjectPlaced(heldObject);

            // 5) Re-enable the object's own collider and drop
            Collider col = heldObject.GetComponent<Collider>();
            col.enabled = true;
            heldObject.Drop();
            heldObject = null;

            // 6) Reset state
            isLookingAtSnapSpot = false;
            currentSnapTarget = null;
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


    private void MakeCloneTheColor(Transform obj)
{
    // get every Renderer in this object hierarchy
    foreach (Renderer r in obj.GetComponentsInChildren<Renderer>())
    {
        // grab a copy of its materials array
        Material[] mats = r.materials;
        // replace each slot with your preview material
        for (int i = 0; i < mats.Length; i++)
            mats[i] = PreviewMaterial;
        // write it back so Unity updates the renderer
        r.materials = mats;
    }
}


    public bool IsHolding() => heldObject != null;

    private IEnumerator PlaceObject(
    Vector3 position,
    Quaternion rotation,
    Transform parentTransform = null
)
    {
        // Parent (or unparent) appropriately
        heldObject.transform.SetParent(parentTransform);

        // Snap into place
        heldObject.transform.position = position;
        heldObject.transform.rotation = rotation;

        // (Tiny pause in case you need to wait a frame)
        yield return null;

        // Re-enable its own collider
        Collider col = heldObject.GetComponent<Collider>();
        col.enabled = true;

        // Finalize drop
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
