using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DEBUG_KillBox : MonoBehaviour
{
    [SerializeField]
    private Transform spawnTarget;

    [SerializeField]
    [HideInInspector]
    private new BoxCollider collider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && spawnTarget != null)
        {
            var playerTransform = other.gameObject.transform;
            playerTransform.position = spawnTarget.position;
            playerTransform.rotation = spawnTarget.rotation;
        }
    }


    // Editor and Gizmos
    void Reset()
    {
        collider = GetComponent<BoxCollider>();
        collider.isTrigger = true;
    }

    private void OnDrawGizmos()
    {
        if (spawnTarget != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(spawnTarget.position, 0.25f);
            Vector3[] lineList = { spawnTarget.position, transform.position };
            Gizmos.DrawLineList(lineList);
        }

        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.DrawCube(collider.center, collider.size);
    }
}
