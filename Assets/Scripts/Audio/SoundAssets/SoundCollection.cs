using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomSound", menuName = "Audio/RandomSound")]
public class SoundCollection : ScriptableObject
{
    [SerializeField]
    AudioClip[] sounds;
    [SerializeField]
    bool inOrder;

    public SoundInstance GetInstance()
    {
        return new SoundInstance(sounds, inOrder);
    }
}
