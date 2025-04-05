using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceMovement : IMoveSet
{
    readonly BalancingBeam beam;

    public BalanceMovement(BalancingBeam beam)
    {
        this.beam = beam;
    }

    public void OnEnter(PlayerCharacterController controller)
    {
        controller.MovespeedModifier = 0.5f;
        controller.CameraController.FOVModifier = 0.1f;
    }

    public void OnExit(PlayerCharacterController controller)
    {
        controller.MovespeedModifier = 1f;
        controller.CameraController.FOVModifier = 1f;
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
            Vector3.Dot(inputWorldMoveDir, beam.ForwardDirection) * beam.ForwardDirection.normalized;
        controller.SetVelocity(moveVector, Space.World, VelocityScaling.Clamp01);
    }
}
