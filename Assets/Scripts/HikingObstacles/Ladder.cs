using UnityEngine;

public class Ladder : MonoBehaviour
{
    [Header("Ladder Direction")]
    [SerializeField]
    private Vector3 localUpDirection = Vector3.up;
    [Header("Gizmos")]
    [SerializeField]
    private Vector3 arrowOffset;

    public Vector3 UpDirection => transform.TransformDirection(localUpDirection.normalized);

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        ExtraGizmos.DrawArrow(transform.position + transform.TransformDirection(arrowOffset), UpDirection);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerCharacterController controller))
        {
            controller.ChangeMoveset(new LadderMovement(this));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out PlayerCharacterController controller))
        {
            controller.ChangeMoveset(new StandardMovement());
        }
    }
}
