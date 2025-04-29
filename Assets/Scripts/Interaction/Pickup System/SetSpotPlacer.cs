using UnityEngine;

public class SetSpotPlacer : MonoBehaviour
{
    public string CorrectName;
    public Transform snapPosition; // Where the object will go if placed here
    public AfterActionEvents afterAction;
    public UnityEngine.Events.UnityEvent AfterPlacementEvent;
    public void OnObjectPlaced(PickupableObject obj)
    {
        if (obj.ObjectName == CorrectName)
        {
            AfterPlacementEvent.Invoke();
        }

        
       
    }

}
