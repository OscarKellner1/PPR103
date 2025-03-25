using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveSet
{
    public void OnEnter();
    public void OnExit();
    public void OnUpdate(ref PlayerInput input, PlayerCharacterController controller);
    public void OnFixedUpdate(ref PlayerInput input, PlayerCharacterController controller);
}
