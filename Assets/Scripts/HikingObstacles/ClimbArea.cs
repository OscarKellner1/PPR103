using UnityEngine;

public class ClimbArea : MonoBehaviour
{
    [Header("Ladder Direction")]
    [SerializeField]
    private Vector3 localUpDirection = Vector3.up;
    [SerializeField]
    private float moveSpeedModifier = 0.8f;
    [Header("Gizmos")]
    [SerializeField]
    private Vector3 arrowOffset;

    public Vector3 UpDirection => transform.TransformDirection(localUpDirection.normalized);
    public float MoveSpeedModifier => moveSpeedModifier;

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


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        ExtraGizmos.DrawArrow(transform.position + transform.TransformDirection(arrowOffset), UpDirection);
    }
}
