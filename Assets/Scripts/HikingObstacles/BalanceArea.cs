using UnityEngine;

public class BalanceArea : MonoBehaviour
{
    [Header("Beam Direction")]
    [SerializeField]
    private Vector3 localForwardDirection = Vector3.forward;
    [Header("Movement Modifications")]
    [SerializeField]
    float moveSpeedModifier = 1.0f;
    [SerializeField]
    bool disableGravity = false;
    [Header("View Modifications")]
    [SerializeField] 
    Vector3 localViewDirection = Vector3.forward;
    [SerializeField]
    bool singleSidedView = false;
    [SerializeField]
    float fovModifier = 0.7f;
    [Header("Gizmos")]
    [SerializeField]
    private Vector3 localArrowOffset = Vector3.up * 2;

    public Vector3 ForwardDirection => transform.TransformDirection(localForwardDirection).normalized;
    public float MoveSpeedModifier => moveSpeedModifier;
    public bool DisableGravity => disableGravity;
    public Vector3 ViewDirection => transform.TransformDirection(localViewDirection).normalized;
    public bool SingleSidedView => singleSidedView;
    public float FovModifier => fovModifier;


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
        Gizmos.color = Color.cyan;
        ExtraGizmos.DrawArrow(transform.position + transform.TransformDirection(localArrowOffset), ForwardDirection);

        Gizmos.color = Color.magenta;
        ExtraGizmos.DrawArrow(transform.position + transform.TransformDirection(localArrowOffset), ViewDirection);
    }
}
