using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod]
    private static void StartUpSequence()
    {
        InputUtility.Initialize();

        SceneManager.LoadScene("Bootstrap", LoadSceneMode.Additive);
    }
}
