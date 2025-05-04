using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MaterialSoundDictionary", menuName = "Audio/MaterialSoundDictionary", order = 1)]
public class MaterialSoundDictionaryAsset : ScriptableObject
{
    [SerializeField]
    private SoundCollection defaultSound;
    [SerializeField]
    private List<MaterialSoundEntry> materialSounds = new();

    public MaterialSoundDictionary GetDictionary()
    {
        MaterialSoundDictionary msDictionary = new(new Dictionary<Material, SoundInstance>(), defaultSound.GetInstance());
        var dictionary = msDictionary.Dictionary;
        foreach (var entry in materialSounds)
        {
            if (dictionary.ContainsKey(entry.Material)) continue;
            dictionary.Add(entry.Material, entry.Sound.GetInstance());
        }
        return msDictionary;
    }
}

[Serializable]
public struct MaterialSoundEntry
{
    [SerializeReference]
    Material material;
    [SerializeReference]
    SoundCollection sound;

    public readonly Material Material => material;
    public readonly SoundCollection Sound => sound;

    public MaterialSoundEntry(Material material, SoundCollection sound)
    {
        this.material = material;
        this.sound = sound;
    }
}
