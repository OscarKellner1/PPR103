using UnityEngine;

public class PickupableObject : MonoBehaviour
{
    public string ObjectName;
    public bool isHeld = false;
    private Rigidbody rb;
    private Collider col;
    public AudioSource audioSource;
    public AudioClip PickupSound;
    public AudioClip DropSound;
    [Space(10)]
    //public SetSpotPlacer SetSpotPlacer;
    public float MaxDwnAngle = 60;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }

    public void PickUp(Transform holdPoint)
    {
        
        audioSource.PlayOneShot(PickupSound);
        isHeld = true;
        rb.isKinematic = true;
        col.enabled = false;
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;
    }

    public void Drop()
    {
        audioSource.PlayOneShot(DropSound);
        isHeld = false;
        
    }

  

    public void Activated()
    {
        if (HeldObjectHandler.Instance.heldObject == null) // Only pick up if not holding anything
        {
            HeldObjectHandler.Instance.HoldObject(this);
        }
    }
}
