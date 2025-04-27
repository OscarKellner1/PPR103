using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialSoundDictionary
{
    public Dictionary<Material, AudioClip> Dictionary { get; private set; }
    public AudioClip DefaultClip { get; private set; }

    public MaterialSoundDictionary(Dictionary<Material, AudioClip> dictionary, AudioClip defaultClip)
    {
        Dictionary = dictionary;
        DefaultClip = defaultClip;
    }

    public AudioClip GetClip(Material mat)
    {
        if (mat  == null) return DefaultClip;

        if (Dictionary.ContainsKey(mat))
        {
            return Dictionary[mat];
        }
        else return DefaultClip;
    }
}
