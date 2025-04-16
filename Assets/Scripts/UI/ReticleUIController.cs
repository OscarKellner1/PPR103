using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReticleUIController : MonoBehaviour
{
    [SerializeField]
    InteractionSystem interactionSystem;
    [SerializeField]
    Image image;

    private void Start()
    {
        image.enabled = false;
    }

    private void OnEnable()
    {
        interactionSystem.OnLookAtChange.AddListener(HandleInteractionLookChange);
    }

    private void HandleInteractionLookChange(InteractionObject interactionObject)
    {
        if (interactionObject == null) image.enabled = false;
        else image.enabled = true;
    }

    private void OnDisable()
    {
        interactionSystem.OnLookAtChange.RemoveListener(HandleInteractionLookChange);
    }
}
