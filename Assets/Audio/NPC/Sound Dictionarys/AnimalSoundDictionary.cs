using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimalSoundDictionary", menuName = "ScriptableObjects/AnimalSoundDictionary", order = 1)]
public class AnimalSoundDictionary : ScriptableObject
{
    // A dictionary to hold animal sounds for different animal types or characters
    public AudioClip[] sounds;  // Array of sounds for a specific animal

    // Method to get a random sound clip from the array
    public AudioClip GetRandomSound()
    {
        if (sounds.Length > 0)
        {
            return sounds[Random.Range(0, sounds.Length)];
        }
        else
        {
            Debug.LogWarning("No sounds found in this dictionary.");
            return null;
        }
    }
}
