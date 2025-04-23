using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera Instance { get; private set; }

    public float FOV
    {
        get => cameras[0].fieldOfView;
        set
        {
            foreach (var cam in cameras)
            {
                cam.fieldOfView = value;
            }
        }
    }

    Camera[] cameras;

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

        cameras = GetComponentsInChildren<Camera>();
    }
}
