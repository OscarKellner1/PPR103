using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Toggles visibility of your bootstrap content based on whether a GameObject
/// named "PlayerCharacter" exists in the newly loaded scene.
/// Attach this to an empty GameObject and assign your bootstrap-content root.
/// </summary>
public class BootstrapVisibilityController : MonoBehaviour
{
    [Tooltip("Root object containing all bootstrap scene content to show/hide")]
    public GameObject bootstrapRoot;

    private void Awake()
    {
        // Persist this controller (and optionally the root) across scene loads
        DontDestroyOnLoad(gameObject);
        if (bootstrapRoot != null)
            DontDestroyOnLoad(bootstrapRoot);

        // Listen for scene loads
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Clean up the event subscription
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        // Initial check on first scene
        UpdateBootstrapVisibility();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateBootstrapVisibility();
    }

    private void UpdateBootstrapVisibility()
    {
        // Look for any active GameObject named "PlayerCharacter"
        bool hasPlayer = GameObject.Find("PlayerCharacter") != null;

        if (bootstrapRoot != null)
        {
            bootstrapRoot.SetActive(hasPlayer);
        }
        else
        {
            // If no root assigned, just toggle all children of this object
            foreach (Transform child in transform)
                child.gameObject.SetActive(hasPlayer);
        }
    }
}
