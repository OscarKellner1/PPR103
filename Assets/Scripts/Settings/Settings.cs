using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static Settings Instance;

    [SerializeField]
    Slider slider;
    [SerializeField]
    GameObject mainMenuButton;
    [SerializeField]
    PlayerCharacterSettings defaultCharacterSettings;

    private bool overrideSensitivity = false;
    private float mouseSensitivity;
    private InputType previousInputType = InputType.None;

    public float MouseSensitivity
    {
        get
        {
            if (overrideSensitivity) return mouseSensitivity;
            else return defaultCharacterSettings.LookSensitiviy;
        }
    }
    [HideInInspector]
    public UnityEvent OnClose = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Debug.Log("Duplicate settings instance. Deleting extra instance.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        slider.value = MouseSensitivity;
        gameObject.SetActive(false);
        slider.onValueChanged.AddListener(SetMouseSensitivity);

        SceneManager.sceneLoaded += OnSceneLoad;
        SetMainMenuButtonVisibilityFromScene(SceneManager.GetActiveScene());
    }

    private void OnSceneLoad(Scene scene, LoadSceneMode loadMode)
    {
        CloseMenu();
        SetMainMenuButtonVisibilityFromScene(scene);
    }

    private void SetMainMenuButtonVisibilityFromScene(Scene scene)
    {
        if (scene.name == "Bootstrap") return; // Ignore bootstrap

        if (scene != null & scene.name == "MainMenu")
        {
            mainMenuButton.SetActive(false);
        }
        else
        {
            mainMenuButton.SetActive(true);
        }
    }

    public void OpenMenu()
    {
        gameObject.SetActive(true);
        previousInputType = InputUtility.InputType;
        InputUtility.SetInputType(InputType.Dialogue);
    }

    public void ToggleMenu()
    {
        bool newState = !gameObject.activeSelf;
        
        if (newState == true) OpenMenu();
        else CloseMenu();
    }

    public void CloseMenu()
    {
        gameObject.SetActive(false);
        OnClose.Invoke();
        InputUtility.SetInputType(previousInputType);
    }

    public void SetMouseSensitivity(float value)
    {
        overrideSensitivity = true;
        mouseSensitivity = value;
        var playerCharacter = GameInfo.GetPlayerCharacter();

        if (playerCharacter != null)
        {
            playerCharacter.LookSensitvity = mouseSensitivity;
        }
    }

    public void ToMainMenu()
    {
        CloseMenu();
        SceneManager.LoadScene("MainMenu");
    }
}
