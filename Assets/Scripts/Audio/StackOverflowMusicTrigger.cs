using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackOverflowMusicTrigger : MonoBehaviour
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
            
            AudioClip backgroundMusic = audioManager.GetBackgroundMusic(1);
            if (backgroundMusic != null)
            {
                audioManager.PlayMusic(backgroundMusic);
            }
            else
            {
                Debug.LogError("Background music at index 4 is not set.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && audioManager != null)
        {
            // Play the desired music when the player exits the trigger zone.
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
