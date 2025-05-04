using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MaterialSoundDictionary
{
    public Dictionary<Material, SoundInstance> Dictionary { get; private set; }
    public SoundInstance DefaultSound { get; private set; }

    public MaterialSoundDictionary(Dictionary<Material, SoundInstance> dictionary, SoundInstance defaultSound)
    {
        Dictionary = dictionary;
        DefaultSound = defaultSound;
    }

    public AudioClip GetClip(Material mat)
    {
        if (mat  == null) return GetDefaultClip();

        if (Dictionary.ContainsKey(mat))
        {
            var clip = Dictionary[mat].GetClip();
            if (clip != null) return clip;
            else return GetDefaultClip();
        }
        else return GetDefaultClip();
    }

    private AudioClip GetDefaultClip()
    {
        if (DefaultSound == null) return null;
        else return DefaultSound.GetClip();
    }
}
