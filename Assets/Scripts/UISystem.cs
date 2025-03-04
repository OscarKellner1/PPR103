using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISystem : MonoBehaviour
{
    public static UISystem Instance;

    // Singleton
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public static GameObject AddElement(GameObject prefab)
    {
        return Instantiate(prefab, Instance.transform);
    }
}
