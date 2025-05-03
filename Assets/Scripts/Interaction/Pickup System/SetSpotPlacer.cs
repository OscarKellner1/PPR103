using UnityEngine;

public class SetSpotPlacer : MonoBehaviour
{
    public string CorrectName;
    public Transform snapPosition; // Where the object will go if placed here
    public AfterActionEvents afterAction;
    public string ActionMethodName;
    public bool GotSomething;
    
    public void OnObjectPlaced(PickupableObject obj)
    {
        if (obj.ObjectName == CorrectName && ActionMethodName != "")
        {
            afterAction.StartInstructions(ActionMethodName);
        }

        
       
    }

}
