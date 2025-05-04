using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuCameraController : MonoBehaviour
{
    

    // Update is called once per frame
    void LateUpdate()
    {
        var camTransform = PlayerCamera.Instance.transform;
        camTransform.SetPositionAndRotation(transform.position, transform.rotation);
    }
}
