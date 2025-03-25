using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
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

    //Components
    private new CapsuleCollider collider; // Used in Gizmos
    private Rigidbody rb;
    private GroundedSystem groundedSystem;
    private CameraController cam;
    private InteractionSystem interactionSystem;

    //Input
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction interactAction;
    private PlayerInput playerInput;

    // Public variables
    public float Movespeed => movespeed;
    public float LookSensitvity => lookSensitivity;
    public float JumpImpulse => jumpImpulse;
    public bool IsGrounded => isGrounded;
    public Rigidbody Rigidbody => rb;
    public GroundedSystem GroundedSystem => groundedSystem;
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
        playerInput.Move = moveAction.ReadValue<Vector3>();
        playerInput.Jump |= jumpAction.triggered;
        playerInput.Interact = interactAction.triggered;

        moveSet.OnUpdate(playerInput, this);
    }

    private void FixedUpdate()
    {
        moveSet.OnFixedUpdate(playerInput, this);
        playerInput.Flush();
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
        Vector3 arrowTip = arrowStart + transform.forward * collider.radius * 2f;
        Gizmos.DrawLine(arrowStart, arrowTip);

        Gizmos.DrawLine(arrowTip, arrowTip - (transform.forward + transform.up) * 0.3f);
        Gizmos.DrawLine(arrowTip, arrowTip - (transform.forward - transform.up) * 0.3f);
        Gizmos.DrawLine(arrowTip, arrowTip - (transform.forward + transform.right) * 0.3f);
        Gizmos.DrawLine(arrowTip, arrowTip - (transform.forward - transform.right) * 0.3f);
    }
}
