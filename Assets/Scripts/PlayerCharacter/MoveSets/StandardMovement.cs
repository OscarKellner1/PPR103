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
            controller.Jump(Vector3.up);
        }
    }

    void RotateView(Vector2 lookInput, PlayerCharacterController controller)
    {
        var camController = controller.CameraController;

        controller.Rotate(Quaternion.Euler(0f, lookInput.x, 0f));
        camController.Pitch = Mathf.Clamp(camController.Pitch - lookInput.y, -89f, 89f);
    }

    void MoveInDirection(Vector2 moveInput, PlayerCharacterController controller)
    {
        Vector3 planarMovement = new Vector3(moveInput.x, 0f, moveInput.y);
        controller.SetVelocity(planarMovement, Space.Self);
    }
}
