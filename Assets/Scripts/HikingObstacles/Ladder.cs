using UnityEngine;

public class Ladder : MonoBehaviour
{
    [Header("Ladder Direction")]
    [SerializeField]
    private Vector3 upDirection = Vector3.up;
    [Header("Gizmos")]
    [SerializeField]
    private Vector3 arrowOffset;

    public Vector3 UpDirectionWorld => transform.TransformDirection(upDirection.normalized);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        ExtraGizmos.DrawArrow(transform.position + transform.TransformDirection(arrowOffset), UpDirectionWorld);
    }
}
