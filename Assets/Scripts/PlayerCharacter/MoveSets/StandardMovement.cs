using UnityEngine;

public class StandardMovement : IMoveSet
{
    public void OnEnter(PlayerCharacterController controller) { }

    public void OnExit(PlayerCharacterController controller) { }

    public void OnUpdate(PlayerInput input, PlayerCharacterController controller)
    {
        if (input.Interact)
        {
            controller.InteractionSystem.TryInteract();
        }
    }

    public void OnFixedUpdate(PlayerInput input, PlayerCharacterController controller)
    {
        RotateView(input.Look, controller);
        MoveInDirection(input.Move, controller);


        if (input.Jump && controller.IsGrounded)
        {
            Jump(controller);
        }
    }

    void RotateView(Vector2 lookInput, PlayerCharacterController controller)
    {
        var rb = controller.Rigidbody;
        var camController = controller.CameraController;

        rb.rotation *= Quaternion.Euler(0f, lookInput.x, 0f);
        camController.Pitch = Mathf.Clamp(camController.Pitch - lookInput.y, -89, 89);
    }

    void MoveInDirection(Vector3 moveInput, PlayerCharacterController controller)
    {
        var movespeed = controller.Movespeed;
        var rb = controller.Rigidbody;
       
        Vector3 planarMovement = controller.transform.rotation * (moveInput * movespeed);
        rb.velocity = new Vector3(planarMovement.x, rb.velocity.y, planarMovement.z);
    }

    void Jump(PlayerCharacterController controller)
    {
        var rb = controller.Rigidbody;
        var jumpImpulse = controller.JumpImpulse;

        rb.AddForce(controller.transform.up * jumpImpulse);
    }
}
