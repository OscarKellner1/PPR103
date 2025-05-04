using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundInstance
{
    readonly AudioClip[] sounds;
    readonly bool inOrder;
    int index;

    public SoundInstance(AudioClip[] sounds, bool inOrder)
    {
        this.sounds = sounds;
        this.inOrder = inOrder;
        index = 0;
    }

    public AudioClip GetClip()
    {
        if (sounds == null) return null;
        if (sounds.Length == 0) return null;

        AudioClip clip;
        if (inOrder)
        {
            clip = sounds[index];
            index = (index + 1) % sounds.Length;
        }
        else
        {
            clip = sounds[Random.Range(0, sounds.Length)];
        }

        return clip;
    }
}
