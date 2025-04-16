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
    [SerializeField]
    InputActionAsset debugInputActionAsset;

    private DebugControls debugControls;
    private InputAction reloadScene;
    private InputAction toggleConsole;
    private InputAction enter;
    private InputAction backspace;

    private bool consoleActive = false;
    private string inputBuffer;

    private void Awake()
    {
        if (!Debug.isDebugBuild)
        {
            Destroy(gameObject);
        }
        else if (instance == null)
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
        debugControls = new DebugControls();
        reloadScene = debugControls.Default.ReloadScene;
        toggleConsole = debugControls.Default.ToggleConsole;
        enter = debugControls.Default.Enter;
        backspace = debugControls.Default.Backspace;
        debugControls.Enable();

        ToggleConsole(consoleActive);
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
        if (isActive)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
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

        // Load new scene by name
        if (arguments[0] == "load" && arguments.Length == 2)
        {
            SceneManager.LoadScene(arguments[1], LoadSceneMode.Single);
        }

        // Reload current scene
        if (arguments[0] == "reload" && arguments.Length == 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
        }

        // Force end dialogue
        if (arguments[0] == "end" && arguments[1] == "dialogue" && arguments.Length == 2)
        {
            DialogueManager.Instance.EndDialogue();
        }

        // Add inventory item
        if (arguments[0] == "give" && arguments.Length == 2)
        {
            InventorySystem.Instance.AddItem(arguments[1], 1);
        }

        // Add multiple items
        if (arguments[0] == "give" && arguments.Length == 3)
        {
            if (int.TryParse(arguments[2], out int amount))
            {
                InventorySystem.Instance.AddItem(arguments[1], amount);
            }
            else
            {
                Debug.LogError("Invaid argument. Expected integer, got " + arguments[2]);
            }
        }

        // Remove inventory item
        if (arguments[0] == "take" && arguments.Length == 2)
        {
            InventorySystem.Instance.RemoveItem(arguments[1], 1);
        }

        // Remove multiple items
        if (arguments[0] == "take" && arguments.Length == 3)
        {
            if (int.TryParse(arguments[2], out int amount))
            {
                InventorySystem.Instance.RemoveItem(arguments[1], amount);
            }
            else
            {
                Debug.LogError("Invaid argument. Expected integer, got " + arguments[2]);
            }
        }

        inputBuffer = string.Empty;
    }
}
