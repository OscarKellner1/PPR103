using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : MonoBehaviour
{
    // Inspector varaibles
    [Header("Movement")]
    [SerializeField]
    private float movespeed = 5f;
    [SerializeField]
    private float lookSensitivity = 0.2f;
    [SerializeField]
    private float jumpImpulse = 2f;

    // State
    private bool isGrounded;
    private IMoveSet moveSet;

    // Components
    private new CapsuleCollider collider; // Used in Gizmos
    private Rigidbody rb;
    private GroundedSystem groundedSystem;
    private CameraController cam;
    private InteractionSystem interactionSystem;

    // Input
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction interactAction;
    private PlayerInput playerInput;

    // Public variables
    public float Movespeed => movespeed;
    public float LookSensitvity => lookSensitivity;
    public float JumpImpulse => jumpImpulse;
    public bool UseGravity
    {
        get { return rb.useGravity; }
        set { rb.useGravity = value; }  
    }
    public bool IsGrounded => isGrounded;
    public Vector3 Velocity => rb.velocity;
    public CameraController CameraController => cam;
    public InteractionSystem InteractionSystem => interactionSystem;

    // Unity Messages
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        groundedSystem = GetComponent<GroundedSystem>();
        cam = GetComponentInChildren<CameraController>();
        interactionSystem = GetComponent<InteractionSystem>();

        moveAction = InputUtility.Controls.Character.Move;
        lookAction = InputUtility.Controls.Character.Look;
        jumpAction = InputUtility.Controls.Character.Jump;
        interactAction = InputUtility.Controls.Character.Interact;
        InputUtility.SetInputType(InputType.Character);

        playerInput = new PlayerInput();
        moveSet = new StandardMovement();
    }

    void Update()
    {
        isGrounded = groundedSystem.CheckGrounded();

        playerInput.Look += lookAction.ReadValue<Vector2>() * lookSensitivity;
        playerInput.Move = moveAction.ReadValue<Vector2>();
        playerInput.Jump |= jumpAction.triggered;
        playerInput.Interact = interactAction.triggered;

        moveSet.OnUpdate(playerInput, this);
    }

    private void FixedUpdate()
    {
        moveSet.OnFixedUpdate(playerInput, this);
        playerInput.Flush();
    }


    // Public interface
    public void ChangeMoveset(IMoveSet newMoveset)
    {
        moveSet.OnExit(this);
        moveSet = newMoveset;
        moveSet.OnEnter(this);
    }

    public void MoveInDirection(Vector3 dir, Space space = Space.World)
    {
        if (space == Space.Self)
        {
            dir = transform.TransformDirection(dir);
        }
        if (UseGravity)
        {
            var planarVelocity = new Vector3(dir.x, 0f, dir.z).normalized * movespeed;
            rb.velocity = planarVelocity + Vector3.up * rb.velocity.y;
        }
        else
        {
            rb.velocity = dir.normalized * movespeed;
        }
    }

    public void Rotate(Quaternion rotation)
    {
        rb.rotation *= rotation;
    }

    public void AddForce(Vector3 force, Space space = Space.World, ForceMode forceMode = ForceMode.Force)
    {
        if (space == Space.Self)
        {
            force = transform.TransformDirection(force);
        }
        rb.AddForce(force, forceMode);
    }

    public void Jump(Vector3 dir, Space space = Space.World)
    {
        dir.Normalize();
        if (space == Space.Self)
        {
            dir = transform.TransformDirection(dir);
        }

        rb.AddForce(dir * jumpImpulse, ForceMode.Force);
    }

    // Gizmos //
    private void OnDrawGizmos()
    {
        if (collider == null) collider = GetComponent<CapsuleCollider>();

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(transform.position + transform.up * collider.radius, collider.radius);
        Gizmos.DrawSphere(transform.position + transform.up * collider.height * 0.5f, collider.radius);
        Gizmos.DrawSphere(transform.position + transform.up * (collider.height - collider.radius), collider.radius);
        
        Gizmos.color = Color.blue;
        Vector3 arrowStart = transform.position + transform.up * collider.height * 0.5f;
        ExtraGizmos.DrawArrow(arrowStart, transform.forward, length: collider.height * 0.5f);

    }
}
