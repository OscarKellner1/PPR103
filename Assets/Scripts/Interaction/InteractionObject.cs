using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InteractionObject : MonoBehaviour
{
    public UnityEvent OnInteract;
    public void Interact()
    {
        if (OnInteract != null)
        {
            OnInteract.Invoke();
        }
        else
        {
            Debug.Log("Something is Null");
        }


        
    }
}
