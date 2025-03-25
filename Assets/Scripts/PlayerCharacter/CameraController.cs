using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera cam;
    public float Pitch = 0f;

    void Start()
    {
        cam = Camera.main;
    }

    void LateUpdate()
    {
        transform.localRotation = Quaternion.Euler(Pitch, 0f, 0f);
        cam.transform.position = transform.position;
        cam.transform.rotation = transform.rotation;
    }
}
