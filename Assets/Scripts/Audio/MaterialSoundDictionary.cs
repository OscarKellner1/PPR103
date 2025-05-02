using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSoundDictionary
{
    public Dictionary<Material, SoundCollection> Dictionary { get; private set; }
    public SoundCollection DefaultSound { get; private set; }

    public MaterialSoundDictionary(Dictionary<Material, SoundCollection> dictionary, SoundCollection defaultSound)
    {
        Dictionary = dictionary;
        DefaultSound = defaultSound;
    }

    public AudioClip GetClip(Material mat)
    {
        if (mat  == null) return DefaultSound.GetClip();

        if (Dictionary.ContainsKey(mat) )
        {
            var clip = Dictionary[mat].GetClip();
            if (clip != null) return clip;
            else return DefaultSound.GetClip();
        }
        else return DefaultSound.GetClip();
    }
}
