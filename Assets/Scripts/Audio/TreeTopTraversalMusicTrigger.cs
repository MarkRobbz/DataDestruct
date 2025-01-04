using UnityEngine;

public class TreeTopTraversalMusicTrigger : MonoBehaviour
{
    private AudioManager audioManager;

    private void Start()
    {
        audioManager = FindObjectOfType<AudioManager>();
        if (audioManager == null)
        {
            Debug.LogError("No AudioManager found in the scene.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && audioManager != null)
        {
            // Play the desired music when the player enters the trigger zone.
            AudioClip backgroundMusic = audioManager.GetBackgroundMusic(2);
            if (backgroundMusic != null)
            {
                audioManager.PlayMusic(backgroundMusic);
            }
            else
            {
                Debug.LogError("Background music at index 2 is not set.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && audioManager != null)
        {
            // Play the default music when the player exits the trigger zone.
            AudioClip backgroundMusic = audioManager.GetBackgroundMusic(0);
            if (backgroundMusic != null)
            {
                audioManager.PlayMusic(backgroundMusic);
            }
            else
            {
                Debug.LogError("Background music at index 0 is not set.");
            }
        }
    }
}