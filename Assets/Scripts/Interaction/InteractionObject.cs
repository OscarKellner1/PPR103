using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Used by interaction system to find interactible objects and then to call methods on that object.
/// </summary>
[RequireComponent(typeof(Collider))]
public class InteractionObject : MonoBehaviour
{
    public UnityEvent OnInteract;
    public void Interact()
    {
        OnInteract.Invoke();
    }
}
