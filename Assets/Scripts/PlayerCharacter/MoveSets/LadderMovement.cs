using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderMovement : IMoveSet
{
    Ladder ladder;

    public LadderMovement(Ladder ladder)
    {
        this.ladder = ladder;
    }

    public void OnEnter(PlayerCharacterController controller) { controller.Rigidbody.useGravity = false; }

    public void OnExit(PlayerCharacterController controller) { controller.Rigidbody.useGravity = true; }

    public void OnUpdate(PlayerInput input, PlayerCharacterController controller) { }

    public void OnFixedUpdate(PlayerInput input, PlayerCharacterController controller)
    {
        throw new System.NotImplementedException();
    }

}
