using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class BalanceMovement : IMoveSet
{
    readonly BalanceArea balanceArea;
    float lookAtConstraint;

    public BalanceMovement(BalanceArea balanceArea)
    {
        this.balanceArea = balanceArea;
    }

    public void OnEnter(PlayerCharacterController controller)
    {
        controller.MovespeedModifier = balanceArea.MoveSpeedModifier;
        controller.CameraController.FOVModifier = balanceArea.FovModifier;
        controller.UseGravity = !balanceArea.DisableGravity;
        if (balanceArea.SingleSidedView)
        {
            lookAtConstraint = 179f;
        }
        else
        {
            lookAtConstraint = 100f;
        }
    }

    public void OnExit(PlayerCharacterController controller)
    {
        controller.MovespeedModifier = 1f;
        controller.CameraController.FOVModifier = 1f;
        controller.UseGravity = true;
    }

    public void OnUpdate(PlayerInput input, PlayerCharacterController controller) { }

    public void OnFixedUpdate(PlayerInput input, PlayerCharacterController controller)
    {
        lookAtConstraint = Mathf.Lerp(lookAtConstraint, 20f, 0.05f);
        RotateView(input.Look, controller);
        BalanceMove(input.Move, controller);
    }

    void RotateView(Vector2 lookInput, PlayerCharacterController controller)
    {
        var camController = controller.CameraController;
        Vector3 viewDirection;
        if (balanceArea.SingleSidedView)
        {
            viewDirection = balanceArea.ViewDirection;
        }
        else
        {
            float dot = Vector3.Dot(controller.transform.forward, balanceArea.ViewDirection);
            viewDirection = new(Mathf.Sign(dot) * balanceArea.ViewDirection.x,
                                balanceArea.ViewDirection.y,
                                Mathf.Sign(dot) * balanceArea.ViewDirection.z);
        }

        float newPitch = Mathf.Clamp(camController.Pitch - lookInput.y, -89f, 89f);
        float newYaw = controller.Rotation.eulerAngles.y + lookInput.x;
        Quaternion rotation = Quaternion.Euler(newPitch, newYaw, 0f);
        Vector3 lookDirection = VectorMath.DirectionClamp(rotation * Vector3.forward, lookAtConstraint, viewDirection);

        controller.LookAt(lookDirection);
    }

    public void BalanceMove(Vector2 moveInput, PlayerCharacterController controller)
    {
        Vector3 balanceForwardXZ = new Vector3(balanceArea.ForwardDirection.x, 0f, balanceArea.ForwardDirection.z).normalized;

        Vector3 worldMove =
            controller.transform.TransformDirection(new Vector3(moveInput.x, 0f, moveInput.y));

        Vector3 forwardMove =
            Vector3.Dot(worldMove, balanceForwardXZ) * balanceForwardXZ;
        Vector3 sideMove = worldMove - forwardMove;

        Vector3 moveVector = forwardMove + sideMove * 0.5f;
        controller.SetVelocity(moveVector, Space.World, VelocityScaling.Clamp01);
    }
}
