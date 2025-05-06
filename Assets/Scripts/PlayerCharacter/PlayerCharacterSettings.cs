using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CharacterSettings", menuName = "CharacterSettings")]
public class PlayerCharacterSettings : ScriptableObject
{
    [SerializeField]
    private float lookSensitivity = 0.2f;

    public float LookSensitiviy => lookSensitivity;
}
