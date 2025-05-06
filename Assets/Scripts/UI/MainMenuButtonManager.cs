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

    public void PressedSettings()
    {
        gameObject.SetActive(false);
        Settings.Instance.OpenMenu();
        Settings.Instance.OnClose.AddListener(ActivateSelf);
    }

    private void ActivateSelf()
    {
        gameObject.SetActive(true);
    }

    public void OnDisable()
    {
        Settings.Instance.OnClose.RemoveListener(ActivateSelf);
    }
}
