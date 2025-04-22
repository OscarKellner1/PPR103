using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceMovement : IMoveSet
{
    readonly BalanceArea beam;

    public BalanceMovement(BalanceArea beam)
    {
        this.beam = beam;
    }

    public void OnEnter(PlayerCharacterController controller)
    {
        controller.MovespeedModifier = beam.MoveSpeedModifier;
        controller.UseGravity = false;
    }

    public void OnExit(PlayerCharacterController controller)
    {
        controller.MovespeedModifier = 1f;
        controller.UseGravity = true;
    }

    public void OnUpdate(PlayerInput input, PlayerCharacterController controller) { }

    public void OnFixedUpdate(PlayerInput input, PlayerCharacterController controller)
    {
        RotateView(input.Look, controller);
        BalanceMove(input.Move, controller);
    }


    void RotateView(Vector2 lookInput, PlayerCharacterController controller)
    {
        var camController = controller.CameraController;

        controller.Rotate(Quaternion.Euler(0f, lookInput.x, 0f));
        camController.Pitch = Mathf.Clamp(camController.Pitch - lookInput.y, -89, 89);
    }

    public void BalanceMove(Vector2 moveInput, PlayerCharacterController controller)
    {
        Vector3 inputWorldMoveDir =
            controller.transform.TransformDirection(new Vector3(moveInput.x, 0f, moveInput.y));

        Vector3 moveVector =
            Vector3.Dot(inputWorldMoveDir, beam.ForwardDirection) * beam.ForwardDirection;
        controller.SetVelocity(moveVector, Space.World, VelocityScaling.Clamp01);
    }
}
