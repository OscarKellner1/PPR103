using UnityEngine;

public class PickupableObject : MonoBehaviour
{
    public string ObjectName;
    public bool isHeld = false;
    private Rigidbody rb;
    private Collider col;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void PickUp(Transform holdPoint)
    {
        isHeld = true;
        rb.isKinematic = true;
        col.enabled = false;
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop(Vector3 position, Quaternion rotation)
    {
        isHeld = false;
        transform.SetParent(null);
        rb.isKinematic = false;
        col.enabled = true;
        transform.position = position;
        transform.rotation = rotation;
    }

    public void SnapTo(Transform target)
    {
        isHeld = true;
        transform.SetParent(null);
        transform.position = target.position;
        transform.rotation = target.rotation;
    }

    public void Activated()
    {
        if (HeldObjectHandler.Instance.heldObject == null) // Only pick up if not holding anything
        {
            HeldObjectHandler.Instance.HoldObject(this);
        }
    }
}
