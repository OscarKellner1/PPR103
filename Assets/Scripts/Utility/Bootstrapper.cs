using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Bootstrapper
{
    [RuntimeInitializeOnLoadMethod]
    private static void LoadBoostrapScene()
    {
        Debug.Log("bootstrap");
        SceneManager.LoadScene("Bootstrap", LoadSceneMode.Additive);
    }
}
