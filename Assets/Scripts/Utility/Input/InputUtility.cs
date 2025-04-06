using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class InputUtility
{
    public static Controls Controls { get; private set; }
    public static InputType InputType { get; private set; }


    public static void Initialize()
    {
        Controls = new Controls();
        if (Debug.isDebugBuild) Controls.Debug.Enable();
    }
   
    public static void SetInputType(InputType newType)
    {
        Controls.Disable();
        if (Debug.isDebugBuild) Controls.Debug.Enable(); // Debug should always be enabled in debug scenarios

        InputType = newType;
        switch (InputType)
        {
            case InputType.Character:
                Controls.Character.Enable();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                break;
            case InputType.Dialogue:
                Controls.Dialogue.Enable();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                break;
            case InputType.UI:
                Controls.UI.Enable();
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                break;
        }
    }
}
