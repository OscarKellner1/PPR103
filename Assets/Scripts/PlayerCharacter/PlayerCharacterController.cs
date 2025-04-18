using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacterController : MonoBehaviour
{
    // Inspector varaibles
    [Header("Movement")]
    [SerializeField]
    private float baseMovespeed = 5f;
    [SerializeField]
    private float lookSensitivity = 0.2f;
    [SerializeField]
    private float jumpImpulse = 2f;

    // State
    private bool isGrounded;
    private float moveSpeedModifier = 1f;
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
    public float BaseMovespeed => baseMovespeed;
    public float Movespeed => baseMovespeed * moveSpeedModifier;
    public float MovespeedModifier
    { 
        get { return moveSpeedModifier; } 
        set { moveSpeedModifier = value; }
    }
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

        // Flushing persistent input
        playerInput.Look = Vector2.zero;
        playerInput.Jump = false;
    }


    // Public interface
    public void ChangeMoveset(IMoveSet newMoveset)
    {
        moveSet.OnExit(this);
        moveSet = newMoveset;
        moveSet.OnEnter(this);
    }

    public void SetVelocity(Vector3 dir, Space space = Space.World, VelocityScaling scalingMode = VelocityScaling.Normalize)
    {
        if (space == Space.Self)
        {
            dir = transform.TransformDirection(dir);
        }
        if (UseGravity)
        {
            var planarVelocity = scalingMode switch
            {
                VelocityScaling.Raw =>
                    new Vector3(dir.x, 0f, dir.z),
                VelocityScaling.Normalize =>
                    new Vector3(dir.x, 0f, dir.z).normalized,
                VelocityScaling.Clamp01 =>
                    Vector3.ClampMagnitude(new Vector3(dir.x, 0f, dir.z), 1f),
                _ => 
                    throw new ArgumentException("Invalid scaling mode"),
            } * Movespeed;
            rb.velocity = planarVelocity + Vector3.up * rb.velocity.y;
        }
        else
        {
            rb.velocity = scalingMode switch
            { 
                VelocityScaling.Raw => dir,
                VelocityScaling.Normalize => dir.normalized,
                VelocityScaling.Clamp01 => Vector3.ClampMagnitude(dir, 1f),
                _ => throw new ArgumentException("Invalid scaling mode"),

            } * Movespeed;
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
    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "Speed") {

            moveSpeedModifier = 3f / 5f;
        }
    }

    private void OnTriggerExit(Collider collision)
    {
        if (collision.gameObject.tag == "Speed")
        {
            moveSpeedModifier = 5f / 5f;
        }
    }
}
