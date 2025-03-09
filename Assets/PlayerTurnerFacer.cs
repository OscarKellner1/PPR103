using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurnerFacer : MonoBehaviour
{
    public Transform target; // Assign the player or target object in the inspector

    void Update()
    {
        if (target != null)
        {
            Vector3 direction = target.position - transform.position;
            direction.y = 0; // Keep only the horizontal direction
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, 180, 0); // Rotate 180 degrees to face correctly
            }
        }
    }
}
