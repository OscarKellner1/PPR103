using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TEST_SoundDictionaryLogger : MonoBehaviour
{
    [SerializeField]
    MaterialSoundDictionaryAsset dictionaryAsset;

    MaterialSoundDictionary dictionary;

    InputAction click;

    private void Start()
    {
        dictionary = dictionaryAsset.GetDictionary();
        click = new InputAction(binding: "<Mouse>/leftButton");
        click.Enable();
    }

    private void Update()
    {
        if (click.WasPerformedThisFrame())
        {
            Log();
        }
    }

    private void Log()
    {
        foreach (var key in dictionary.Dictionary.Keys)
        {
            Debug.Log(key);
        }
    }
}
