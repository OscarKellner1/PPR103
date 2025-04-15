using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayModeDebug : MonoBehaviour
{
    private PlayModeDebug instance;

    [SerializeField]
    GameObject DebugConsole;
    [SerializeField]
    TMP_Text textField;

    private InputAction reloadScene;
    private InputAction toggleConsole;
    private InputAction enter;
    private InputAction backspace;

    private bool consoleActive = false;
    private string inputBuffer;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        reloadScene = InputUtility.Controls.Debug.ReloadScene;
        toggleConsole = InputUtility.Controls.Debug.ToggleConsole;
        enter = InputUtility.Controls.Debug.Enter;
        backspace = InputUtility.Controls.Debug.Backspace;
        ToggleConsole(false);
        inputBuffer = string.Empty;
        Keyboard.current.onTextInput += AppendToInputBuffer;

    }

    void Update()
    {
        if (reloadScene.triggered)
        {
            ReloadScene();
        }
        if (toggleConsole.triggered)
        {
            ToggleConsole(!consoleActive);
        }

        if (consoleActive)
        {
            textField.text = inputBuffer;

            if (backspace.triggered) DeleteLastFromInputBuffer();
            if (enter.triggered) HandleCommand();
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    void ToggleConsole(bool isActive)
    {
        DebugConsole.SetActive(isActive);
        InputUtility.SetActive(!isActive);
        consoleActive = isActive;
    }

    void AppendToInputBuffer(char c)
    {
        if (!consoleActive) return;
        if (char.IsControl(c)) return;
        if (c == '*') return;

        inputBuffer = inputBuffer + c;
    }

    void DeleteLastFromInputBuffer()
    {
        if (inputBuffer.Length == 0) return;
        inputBuffer = inputBuffer.Remove(inputBuffer.Length - 1);
    }

    void HandleCommand()
    {
        var arguments = inputBuffer.Split(' ');
        if (arguments[0] == "scenes" && arguments.Length == 1)
        {
            for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
            {
                var scene = SceneManager.GetSceneByBuildIndex(i);
                Debug.Log("Scene " + i + ": " + scene.name);
            }
        }
        if (arguments[0] == "load" && arguments.Length == 2)
        {
            SceneManager.LoadScene(arguments[1], LoadSceneMode.Single);
        }

        inputBuffer = string.Empty;
    }
}
