using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundCollection", menuName = "Audio/SoundCollection")]
public class SoundCollection : ScriptableObject
{
    [SerializeField]
    AudioClip[] sounds;

    public AudioClip GetClip()
    {
        if (sounds.Length == 0) return null;

        return sounds[Random.Range(0, sounds.Length)];
    }
}
