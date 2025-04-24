using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "MaterialSoundDictionary", menuName = "MaterialSoundDictionary", order = 1)]
public class MaterialSoundDictionaryAsset : ScriptableObject
{
    [SerializeField]
    private AudioClip defaultClip;
    [SerializeField]
    private List<MaterialSoundEntry> materialSounds = new();

    public MaterialSoundDictionary GetDictionary()
    {
        MaterialSoundDictionary msDictionary = new(new Dictionary<Material, AudioClip>(), defaultClip);
        var dictionary = msDictionary.Dictionary;
        foreach (var entry in materialSounds)
        {
            if (dictionary.ContainsKey(entry.Material)) continue;
            dictionary.Add(entry.Material, entry.Clip);
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
    AudioClip clip;

    public readonly Material Material => material;
    public readonly AudioClip Clip => clip;

    public MaterialSoundEntry(Material material, AudioClip clip)
    {
        this.material = material;
        this.clip = clip;
    }
}
