using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class InputUtility
{
    public static Controls Controls { get; private set; }
    private static InputType type;


    public static void Initialize()
    {
        Controls = new Controls();
    }

    public static InputType GetInputType()
    {
        return type;
    }

    public static void SetInputType(InputType newType)
    {
        Controls.Disable();
        type = newType;
        switch (newType)
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
