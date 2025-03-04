using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DEBUG_Interactible : MonoBehaviour
{
    private InteractionObject interactionPoint;
    private void Awake()
    {
        interactionPoint = GetComponent<InteractionObject>();
    }

    private void OnEnable()
    {
        interactionPoint.OnInteract.AddListener(RandomRotate);
    }

    private void OnDisable()
    {
        interactionPoint.OnInteract.RemoveListener(RandomRotate);
    }

    private void RandomRotate()
    {
        transform.rotation = Quaternion.Euler(
            Random.Range(0f, 360f), 
            Random.Range(0f, 360f), 
            Random.Range(0f, 360f));
    }
}
