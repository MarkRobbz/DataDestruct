using UnityEngine;

public class MainMenuSong : MonoBehaviour
{
    public AudioClip menuMusicClip; // Assign main menu song clip in the inspector

    void Start()
    {
        // Find the AudioManager that exists in the current scene
        AudioManager audioManager = FindObjectOfType<AudioManager>();
        if (audioManager != null && menuMusicClip != null)
        {
            audioManager.PlayMusic(menuMusicClip, true); // Play the menu music clip
        }
        else
        {
            Debug.LogError("AudioManager instance not found or no menu music clip specified.");
        }
    }
}