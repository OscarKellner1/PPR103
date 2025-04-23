using UnityEngine;

public class BalanceArea : MonoBehaviour
{
    [Header("Trigger")]
    [SerializeField]
    BoxCollider triggerCollider;
    [Header("Beam Direction")]
    [SerializeField]
    private Vector3 localForwardDirection = Vector3.forward;
    [Header("Movement Modifications")]
    [SerializeField]
    float moveSpeedModifier = 1.0f;
    [Header("Gizmos")]
    [SerializeField]
    private Vector3 arrowOffset;

    public Vector3 ForwardDirection => transform.TransformDirection(localForwardDirection).normalized;
    public float MoveSpeedModifier => moveSpeedModifier;


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out PlayerCharacterController controller))
        {
            controller.ChangeMoveset(new BalanceMovement(this));
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
        ExtraGizmos.DrawArrow(transform.position + transform.TransformDirection(arrowOffset), ForwardDirection);
    }
}
