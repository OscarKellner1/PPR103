using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    float bobSpeed = 2f;
    [SerializeField]
    float bobAmplitude = 0.15f;
    [SerializeField]
    DynamicValue bobAmount;

    private Camera cam;
    private PlayerCharacterController charController;
    private Vector3 cameraOffset;
    private float pitch = 0;
    
    private float bobAnimationValue;

    public float Pitch
    {
        get { return pitch; }
        set { transform.localRotation = Quaternion.Euler(value, 0f, 0f); pitch = value; }
    }



    void Start()
    {
        cam = Camera.main;
        charController = GetComponentInParent<PlayerCharacterController>();
        Pitch = 0f;
    }


    void LateUpdate()
    {
        if (charController.Velocity.magnitude > 0f )
        {
            bobAmount.Increase(Time.deltaTime);
        }
        else
        {
            bobAmount.Decrease(Time.deltaTime);
        }

        bobAnimationValue += Time.deltaTime * bobSpeed * charController.Velocity.magnitude;
        cameraOffset = Vector3.up * Mathf.Sin(bobAnimationValue) * bobAmplitude * bobAmount.Value;

        UpdateCamera();
    }

    void UpdateCamera()
    {
        cam.transform.position = transform.position + cameraOffset;
        cam.transform.rotation = transform.rotation;
    }
}
