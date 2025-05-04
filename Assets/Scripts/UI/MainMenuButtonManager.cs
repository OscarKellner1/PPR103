using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtonManager : MonoBehaviour
{
    public void Start()
    {
        InputUtility.SetInputType(InputType.Dialogue);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }
    public void PressedPlay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void PressedQuit()
    {
        Application.Quit();
    }
}
