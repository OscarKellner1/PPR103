using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera cam;

    private float baseFOV;

    public float Pitch { get; set; } = 0f;
    public float FOVModifier { get; set; } = 1.0f;

    void Start()
    {
        cam = Camera.main;
        baseFOV = cam.fieldOfView;
    }

    void LateUpdate()
    {
        transform.localRotation = Quaternion.Euler(Pitch, 0f, 0f);
        cam.transform.position = transform.position;
        cam.transform.rotation = transform.rotation;
        Debug.Log(FOVModifier);
    }
}
