using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterTapCustomScript : MonoBehaviour
{
  public Animation BucketAnim;
    public GameObject SetSpotPlacer;

    public void StartWatering()
    {
        BucketAnim.Play();
    }

    public void StopWatering()
    {
        BucketAnim.gameObject.GetComponent<PickupableObject>().ObjectName = "Water Bucket";
        foreach (Transform t in SetSpotPlacer.GetComponentsInChildren<Transform>(includeInactive: true))
        {
            // skip the root itself
            if (t == SetSpotPlacer.transform)
                continue;

            if (t.name == "Bucket")
            {
                t.SetParent(null, worldPositionStays: true);
                Destroy(SetSpotPlacer);
                return;
            }
        }
    }
}
