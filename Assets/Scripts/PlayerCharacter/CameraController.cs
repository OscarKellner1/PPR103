using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [Header("Camera bobbing")]
    [SerializeField]
    float bobSpeed = 2f;
    [SerializeField]
    float bobAmplitude = 0.15f;
    [SerializeField]
    AnimationValue bobAnimation;
    [Header("FOV")]
    [SerializeField]
    SmoothedValue fov = new(60f, 1f);

    private PlayerCharacterController charController;
    private Vector3 cameraOffset;
    private float pitch = 0;
    private float fovModifier = 1;
    private float bobAnimationValue;

    public float Pitch
    {
        get { return pitch; }
        set { pitch = ExtraMathf.Mod(value - 180f, 360f) - 180f; transform.localRotation = Quaternion.Euler(pitch, 0f, 0f); }
    }

    public float FOVModifier
    {
        get { return fovModifier; }
        set { fovModifier = value; }
    }


    void Start()
    {
        charController = GetComponentInParent<PlayerCharacterController>();
        Pitch = 0f;
    }


    void LateUpdate()
    {
        fov.MoveToward(fov.Initial * fovModifier);

        if (charController.Velocity.magnitude > 0.01f && charController.IsGrounded)
        {
            bobAnimation.Increase(Time.deltaTime);
        }
        else
        {
            bobAnimation.Decrease(Time.deltaTime);
        }

        bobAnimationValue += Time.deltaTime * bobSpeed * charController.Velocity.magnitude;
        cameraOffset = bobAnimation.Value * bobAmplitude * Mathf.Sin(bobAnimationValue) * Vector3.up;

        UpdateCamera();
    }

    void UpdateCamera()
    {
        PlayerCamera.Instance.FOV = fov.Current;
        var camTransform = PlayerCamera.Instance.transform;
        camTransform.SetPositionAndRotation(transform.position + cameraOffset, transform.rotation);
    }
}
