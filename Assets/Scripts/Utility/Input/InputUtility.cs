using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class InputUtility
{
    public static Controls Controls { get; private set; }
    public static InputType InputType { get; private set; }
    public static bool InDebugMode { get; private set; }

    private static InputType previousInputType;

    public static void Initialize()
    {
        Controls = new Controls();
        InDebugMode = false;
        if (Debug.isDebugBuild) Controls.Debug.Enable();
    }
   
    public static void SetInputType(InputType newType)
    {
        if (!InDebugMode) return;

        Controls.Disable();
        if (Debug.isDebugBuild) Controls.Debug.Enable(); // Debug should always be enabled in debug builds

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
        }
    }

    public static void SetDebugMode(bool active)
    {
        if (!Debug.isDebugBuild) return;

        if (InDebugMode == active) return;
        InDebugMode = active;

        if (active)
        {
            previousInputType = InputType;
            Controls.Disable();
            Controls.Debug.Enable(); // Make sure debug is still active
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            SetInputType(previousInputType);
        }
    }
}
