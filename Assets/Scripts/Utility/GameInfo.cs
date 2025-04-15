using UnityEngine;

public class GameInfo : MonoBehaviour
{
    private GameInfo instance;
    static GameObject playerCharacter;

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

    public static GameObject GetPlayerCharacter()
    {
        if (playerCharacter == null)
        {
            playerCharacter = GameObject.Find("PlayerCharacter");
        }

        return playerCharacter;
    }
}
