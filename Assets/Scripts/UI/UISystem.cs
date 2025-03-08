using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : MonoBehaviour
{
    private static UISystem instance;

    // Singleton
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static GameObject AddElement(GameObject prefab)
    {
        GameObject gameObject = Instantiate(prefab);
        gameObject.transform.SetParent(instance.transform, false);
        return gameObject;
    }
}
