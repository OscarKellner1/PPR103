using UnityEngine;
using Cinemachine;
using System.Collections;

public class CinemachineManager : MonoBehaviour
{
    public static CinemachineManager Instance;

    public Transform playerCamera;
    public float lookSpeed = 2f;

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
        StartCoroutine(SmoothLookAt(newTransform));
    }

    public IEnumerator SmoothLookAt(Transform target)
    {
        while (true)
        {
            Debug.Log("Looked");
            Vector3 direction = (target.position - playerCamera.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            playerCamera.rotation = Quaternion.Slerp(playerCamera.rotation, lookRotation, Time.deltaTime * lookSpeed);

            // Stop when close enough
            if (Quaternion.Angle(playerCamera.rotation, lookRotation) < 1f)
                yield break;

            yield return null;
        }
       
    }
}
