using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct PlayerInput
{
    public Vector2 Look;
    public Vector3 Move;
    public bool Interact;
    public bool Jump;

    public void Flush()
    {
        Look = Vector2.zero;
        Move = Vector3.zero;
        Interact = false;
        Jump = false;
    }
}
