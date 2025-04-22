using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : IMoveSet
{
    readonly Ladder ladder;

    public LadderMovement(Ladder ladder)
    {
        this.ladder = ladder;
    }

    public void OnEnter(PlayerCharacterController controller) 
    { 
        controller.UseGravity = false;
        controller.MovespeedModifier = ladder.MoveSpeedModifier;
    }

    public void OnExit(PlayerCharacterController controller) 
    { 
        controller.UseGravity = true;
        controller.MovespeedModifier = 1f;
        controller.SetVelocity(
            new Vector3(controller.Velocity.x, 0f, controller.Velocity.y ), Space.World, VelocityScaling.Raw);
    }

    public void OnUpdate(PlayerInput input, PlayerCharacterController controller) { }

    public void OnFixedUpdate(PlayerInput input, PlayerCharacterController controller)
    {
        RotateView(input.Look, controller);
        LadderMove(input.Move, controller);
    }

    void RotateView(Vector2 lookInput, PlayerCharacterController controller)
    {
        var camController = controller.CameraController;

        controller.Rotate(Quaternion.Euler(0f, lookInput.x, 0f));
        camController.Pitch = Mathf.Clamp(camController.Pitch - lookInput.y, -89, 89);
    }

    void LadderMove(Vector3 moveInput, PlayerCharacterController controller)
    {
        Vector3 ladderMovement;

        if (controller.IsGrounded && moveInput.y < 0)
        {
            ladderMovement =
                controller.transform.TransformDirection(new Vector3(moveInput.x, 0f, moveInput.y));
        }
        else
        {
            ladderMovement =
                moveInput.y * ladder.UpDirection
                + moveInput.x * controller.transform.right;
        }

        controller.SetVelocity(ladderMovement);
    }

}
