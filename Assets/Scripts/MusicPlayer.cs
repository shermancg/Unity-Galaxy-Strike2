using UnityEngine;

public class MusicPlayer : MonoBehaviour
{

    void Start()
    {
        // Check if there are any other MusicPlayer instances in the scene
        int numOfMusicPlayers = FindObjectsByType<MusicPlayer>(FindObjectsSortMode.None).Length;

        if (numOfMusicPlayers > 1)
        {
            // If there is more than one, destroy this instance
            Destroy(gameObject);
        }
        else
        {
            // If this is the only instance, make it persistent across scenes
            DontDestroyOnLoad(gameObject);
        }        
    }

}
