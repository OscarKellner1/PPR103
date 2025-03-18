using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreDebug : MonoBehaviour
{
    private static MoreDebug instance;
    [SerializeField]
    private GameObject debugSphere;
    private Dictionary<int, GameObject> debugDict = new Dictionary<int, GameObject>();
    private int debugCount = 0;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    public static int SpawnDebugSphere(Vector3 position, float size, float time)
    {
        var sphere = Instantiate(instance.debugSphere, position, Quaternion.identity);
        sphere.transform.localScale = Vector3.one * size;
        Destroy(sphere, time);

        int debugID = instance.debugCount;
        instance.debugDict[debugID] = sphere;
        instance.debugCount++;
        return debugID;
    }
}
