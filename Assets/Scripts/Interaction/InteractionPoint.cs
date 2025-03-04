using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InteractionPoint : MonoBehaviour
{
    public UnityEvent OnInteract;
    public void Interact()
    {
        OnInteract.Invoke();
    }
}
