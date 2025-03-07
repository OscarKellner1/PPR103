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

    [Header("Grounded Check")]
    [SerializeField]
    private float groundedRaycastLength = 0.1f;
    [SerializeField]
    private LayerMask groundedRaycastMask;

    // Hidden varaibles
    private float cameraPitch;
    private bool isGrounded;
    private Vector2 lookInput;
    private Vector3 moveInput;
    private bool jumpInput;

    //Components
    private new CapsuleCollider collider; // Used in Gizmos
    private Rigidbody rb;
    private Camera cam;
    private InteractionSystem interactionSystem;

    //Input system
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction jumpAction;
    private InputAction interactAction;


    // Unity Messages
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        interactionSystem = GetComponent<InteractionSystem>();

        moveAction = InputUtility.Controls.Character.Move;
        lookAction = InputUtility.Controls.Character.Look;
        jumpAction = InputUtility.Controls.Character.Jump;
        interactAction = InputUtility.Controls.Character.Interact;
        InputUtility.SetInputType(InputType.Character);
    }

    void Update()
    {
        isGrounded = CheckGrounded();

        // Physics related input
        lookInput += lookAction.ReadValue<Vector2>() * lookSensitivity;
        moveInput = moveAction.ReadValue<Vector3>();
        jumpInput |= jumpAction.triggered;

        if (interactAction.triggered)
        {
            interactionSystem.TryInteract();
        }

    }

    private void FixedUpdate()
    {
        RotateView(lookInput);
        lookInput = Vector2.zero;

        MoveInDirection(moveInput);


        if (jumpInput && isGrounded)
        {
            Jump();
        }
        jumpInput = false;
    }


    // Helper Methods
    bool CheckGrounded()
    {
        Ray ray = new(transform.position + transform.up * 0.1f, -transform.up);
        Debug.DrawRay(ray.origin, ray.direction, Color.cyan);
        return Physics.Raycast(ray, groundedRaycastLength + 0.1f, groundedRaycastMask);
    }

    void RotateView(Vector2 input)
    {
        rb.rotation *= Quaternion.Euler(0f, input.x, 0f);
        cameraPitch = Mathf.Clamp(cameraPitch - input.y, -89, 89);
        cam.transform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);
    }

    void MoveInDirection(Vector3 input)
    {
        Vector3 planarMovement = transform.rotation * (input * movespeed);
        rb.velocity = new Vector3(planarMovement.x, rb.velocity.y, planarMovement.z);
    }

    void Jump()
    {
        rb.AddForce(transform.up * jumpImpulse);
    }


    // Gizmos
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
