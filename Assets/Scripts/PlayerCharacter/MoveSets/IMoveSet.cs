using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A moveset in this context is a set of instructions for how a character should move given certain input. The moveset of the 
/// PlayerCharacterController has full controll over how the character moves.
/// 
/// Be careful to perform the correct actions in the correct update. Any physics updates like rotation and movement should happen
/// in the OnFixedUpdate method. In fact certan types of input such as the jump input will not work properly if used in Update.
/// </summary>
public interface IMoveSet
{
    /// <summary>
    /// Transition method. Called when the moveset is first entered.
    /// </summary>
    public void OnEnter(PlayerCharacterController controller);
    /// <summary>
    /// Transition method. Called on exiting the moveset.
    /// </summary>
    public void OnExit(PlayerCharacterController controller);
    /// <summary>
    /// Called during PlayerCharacterController Update. Do not use for physics.
    /// </summary>
    public void OnUpdate(PlayerInput input, PlayerCharacterController controller);
    /// <summary>
    /// Called during PlayerCharacterController FixedUpdate.
    /// </summary>
    public void OnFixedUpdate(PlayerInput input, PlayerCharacterController controller);
}
