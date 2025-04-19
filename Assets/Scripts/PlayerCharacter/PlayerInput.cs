using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple struct for holding and manipulating player input.
/// </summary>
public struct PlayerInput
{
    public Vector2 Look;
    public Vector2 Move;
    public bool Interact;
    public bool Jump;
}
