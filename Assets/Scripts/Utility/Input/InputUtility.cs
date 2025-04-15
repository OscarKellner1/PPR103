using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class InputUtility
{
    public static Controls Controls { get; private set; }
    public static InputType InputType { get; private set; }
    public static bool InDebugMode { get; private set; }

    private static bool isActive = true;

    public static void Initialize()
    {
        Controls = new Controls();
        InDebugMode = false;
        SetInputType(InputType.None);
    }
   
    public static void SetInputType(InputType newType)
    {
        Controls.Disable();
        InputType = newType; // Still assigns new type so that eventual changes in input type will be applied when activating again

        if (!isActive ) return;

        switch (InputType)
        {
            case InputType.None:
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.None;
                break;
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

    public static void SetActive(bool active)
    {
        if (active)
        {
            isActive = true;
            SetInputType(InputType);
        }
        else
        {
            Controls.Disable();
            isActive = false;
        }
    }
}
