using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveSet
{
    public void OnEnter();
    public void OnExit();
    public void OnUpdate(PlayerInput input, PlayerCharacterController controller);
    public void OnFixedUpdate(PlayerInput input, PlayerCharacterController controller);
}
