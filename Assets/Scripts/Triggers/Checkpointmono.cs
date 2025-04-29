using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpointmono : MonoBehaviour
{
    private Vector3 spawn => transform.position;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player"){
            Debug.Log ("Checkpoint!");
            other.GetComponent<PlayerRespawn>().SetSpawnPoint(spawn);
        }
    }
}
