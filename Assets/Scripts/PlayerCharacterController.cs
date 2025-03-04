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


    // Hidden varaibles
    private float cameraPitch;

    //Components
    private new CapsuleCollider collider; // Used in Gizmos
    private Rigidbody rb;
    private Camera cam;
    private InteractionSystem interactionSystem;

    //Input system
    private Controls controls;
    private InputAction moveAction;
    private InputAction lookAction;
    private InputAction interactAction;


    void Start()
    {
        rb = GetComponent<Rigidbody>();
        cam = GetComponentInChildren<Camera>();
        interactionSystem = GetComponent<InteractionSystem>();

        controls = new Controls();
        moveAction = controls.FindAction("Move", throwIfNotFound: true);
        lookAction = controls.FindAction("Look", throwIfNotFound: true);
        interactAction = controls.FindAction("Interact", throwIfNotFound: true);
        controls.Character.Enable();
    }

    void Update()
    {
        //Looking around
        var lookInput = lookAction.ReadValue<Vector2>() * lookSensitivity;
        rb.rotation *= Quaternion.Euler(0f, lookInput.x, 0f);
        cameraPitch = Mathf.Clamp(cameraPitch - lookInput.y, -89, 89);
        cam.transform.localRotation = Quaternion.Euler(cameraPitch, 0f, 0f);

        //Movement
        var moveInput = moveAction.ReadValue<Vector3>();
        rb.velocity = transform.rotation * (moveInput * movespeed);

        //Interaction
        if (interactAction.triggered)
        {
            interactionSystem.TryInteract();
        }
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
