using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    public Vector3 SpawnPoint;
    public void SetSpawnPoint(Vector3 SpawnChange)
    {
        SpawnPoint = SpawnChange;
    }

    private void SpawnHere()
    {
        transform.position = SpawnPoint;
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Respawn")
        {
            SpawnHere();
        }
    }
}
