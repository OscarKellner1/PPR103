using UnityEngine;
using Cinemachine;

public class CinemachineManager : MonoBehaviour
{
    public static CinemachineManager Instance;

    public CinemachineVirtualCamera currentCamera;

    void Awake()
    {
        // Ensure there's only one instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SwitchCamera(Transform newTransform)
    {
        Debug.Log("I'm looking here: " +  newTransform);
        if (currentCamera != null)
        {
            // Update the camera's position or look at target
        
            currentCamera.LookAt = newTransform;
        }
    }
}
