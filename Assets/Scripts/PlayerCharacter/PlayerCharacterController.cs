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
    [SerializeField]
    private float slopeAngleThreshold = 45f;

    // State
    private GroundCheckResult groundCheck = GroundCheckResult.NoHit();
    private float moveSpeedModifier = 1f;
    private IMoveSet moveSet;

    // Components and related
    private new CapsuleCollider collider;
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
    public float SlopeAngleThreshold => slopeAngleThreshold;
    public bool UseGravity
    {
        get { return rb.useGravity; }
        set { rb.useGravity = value; }  
    }
    public bool IsGrounded => groundCheck.hit && !SlopeExceedsThreshold;
    public bool SlopeExceedsThreshold =>
        Vector3.Dot(groundCheck.normal, Vector3.up) < Mathf.Cos(Mathf.Deg2Rad*slopeAngleThreshold);
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
        collider = GetComponent<CapsuleCollider>();

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
        playerInput.Look += lookAction.ReadValue<Vector2>() * lookSensitivity;
        playerInput.Move = moveAction.ReadValue<Vector2>();
        playerInput.Jump |= jumpAction.triggered;
        playerInput.Interact = interactAction.triggered;

        moveSet.OnUpdate(playerInput, this);
    }

    private void FixedUpdate()
    {
        groundCheck = groundedSystem.CheckGrounded();

        // Adjust friction
        if (IsGrounded && playerInput.Move == Vector2.zero)
        {
            collider.material.staticFriction = 100;
            collider.material.dynamicFriction = 100;
            collider.material.frictionCombine = PhysicMaterialCombine.Maximum;
        }
        else
        {
            collider.material.staticFriction = 0f;
            collider.material.dynamicFriction = 0f;
            collider.material.frictionCombine = PhysicMaterialCombine.Minimum;
        }

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

        Vector3 newVelocity;
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
            newVelocity = planarVelocity + Vector3.up * rb.velocity.y;
        }
        else
        {
            newVelocity = scalingMode switch
            { 
                VelocityScaling.Raw => dir,
                VelocityScaling.Normalize => dir.normalized,
                VelocityScaling.Clamp01 => Vector3.ClampMagnitude(dir, 1f),
                _ => throw new ArgumentException("Invalid scaling mode"),

            } * Movespeed;
        }

        if (UseGravity && groundCheck.hit)
        {
            bool movingIntoSLope = Vector3.Dot(groundCheck.normal, newVelocity) < 0f;
            if (SlopeExceedsThreshold && movingIntoSLope)
            {
                // Find vector tangent to slope
                Vector3 tangent = Vector3.Cross(groundCheck.normal, Vector3.up);
                // Project velocity onto tangent
                newVelocity = tangent * Vector3.Dot(newVelocity, tangent);
                newVelocity = new Vector3(newVelocity.x, rb.velocity.y, newVelocity.z);
            }
        }
        

        rb.velocity = newVelocity;
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
