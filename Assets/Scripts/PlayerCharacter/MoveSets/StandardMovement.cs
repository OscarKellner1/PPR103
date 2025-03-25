using UnityEngine;

public class StandardMovement : IMoveSet
{
    public void OnEnter() { }

    public void OnExit() { }

    public void OnUpdate(ref PlayerInput input, PlayerCharacterController controller)
    {
        if (input.Interact)
        {
            controller.InteractionSystem.TryInteract();
        }
    }

    public void OnFixedUpdate(ref PlayerInput input, PlayerCharacterController controller)
    {
        RotateView(input.Look, controller);
        input.Look = Vector2.zero;

        MoveInDirection(input.Move, controller);


        if (input.Jump && controller.IsGrounded)
        {
            Jump(controller);
        }
        input.Jump = false;
    }

    void RotateView(Vector2 input, PlayerCharacterController controller)
    {
        var rb = controller.Rigidbody;
        var camController = controller.CameraController;

        rb.rotation *= Quaternion.Euler(0f, input.x, 0f);
        camController.Pitch = Mathf.Clamp(camController.Pitch - input.y, -89, 89);
    }

    void MoveInDirection(Vector3 input, PlayerCharacterController controller)
    {
        var movespeed = controller.Movespeed;
        var rb = controller.Rigidbody;
       
        Vector3 planarMovement = controller.transform.rotation * (input * movespeed);
        rb.velocity = new Vector3(planarMovement.x, rb.velocity.y, planarMovement.z);
    }

    void Jump(PlayerCharacterController controller)
    {
        var rb = controller.Rigidbody;
        var jumpImpulse = controller.JumpImpulse;

        rb.AddForce(controller.transform.up * jumpImpulse);
    }
}
