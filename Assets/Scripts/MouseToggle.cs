using UnityEngine;

public class MouseToggle : MonoBehaviour
{
    private bool isMouseLocked = true; // Track if the mouse is currently locked or not

    private void Update()
    {
        // Toggle mouse lock and visibility when the "F" key is pressed
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isMouseLocked)
            {
                UnlockMouse();
            }
            else
            {
                LockMouse();
            }
        }
    }

    public void UnlockMouse()
    {
        // Unlock the mouse and make it visible
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        isMouseLocked = false; // Update the state
    }

    public void LockMouse()
    {
        // Lock the mouse and hide it
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        isMouseLocked = true; // Update the state
    }
}
