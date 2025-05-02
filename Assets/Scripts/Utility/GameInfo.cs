using UnityEngine;

public class GameInfo : MonoBehaviour
{
    private GameInfo instance;
    static PlayerCharacterController playerCharacter;

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

    public static PlayerCharacterController GetPlayerCharacter()
    {
        if (playerCharacter == null)
        {
            playerCharacter = FindAnyObjectByType<PlayerCharacterController>(FindObjectsInactive.Include);
        }

        return playerCharacter;
    }
}
