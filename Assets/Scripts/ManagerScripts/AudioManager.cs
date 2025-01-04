using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource musicSource; // global music
    public AudioSource sfxSource; // global sound effects

    public AudioClip[] backgroundMusic; // Array for background music
    public AudioClip[] soundEffects; // Array for sound effects

    // Start is now only needed if you plan to do some initialization checks
    void Start()
    {
        // Example initialization check for the background music array
        if (backgroundMusic != null)
        {
            Debug.Log($"Background music array is loaded with {backgroundMusic.Length} elements.");
        }
        else
        {
            Debug.LogError("Background music array is not loaded properly.");
        }
    }

    // Plays music from the musicSource AudioSource
    public void PlayMusic(AudioClip musicClip, bool loop = true)
    {
        Debug.Log("PlayMusic called with clip: " + (musicClip ? musicClip.name : "null"));

        if (musicClip == null)
        {
            Debug.LogError("Music clip is null");
            return;
        }

        musicSource.clip = musicClip;
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // Plays global sound effect from sfxSource AudioSource
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }

    // Plays a sound effect at a specific location in the world
    public void PlaySoundAtLocation(AudioClip clip, Vector3 position, float volume = 1.0f)
    {
        AudioSource.PlayClipAtPoint(clip, position, volume);
    }

    // Plays sound using a given AudioSource (positional audio on specific GameObjects)
    public void PlaySoundOnSource(AudioClip clip, AudioSource source, float volume = 1.0f)
    {
        if (source == null)
        {
            Debug.LogError("AudioSource is null.");
            return;
        }
        source.PlayOneShot(clip, volume);
    }

    // Gets background music by index, directly from the background music array
    public AudioClip GetBackgroundMusic(int index)
    {
        if (backgroundMusic == null)
        {
            Debug.LogError("Background music array is null.");
            return null;
        }
        if (index >= 0 && index < backgroundMusic.Length)
        {
            return backgroundMusic[index];
        }
        else
        {
            Debug.LogError($"Index {index} out of range for background music.");
            return null;
        }
    }

    // Gets a sound effect by index, directly from the sound effects array
    public AudioClip GetSoundEffect(int index)
    {
        if (soundEffects == null)
        {
            Debug.LogError("Sound effects array is null.");
            return null;
        }
        if (index >= 0 && index < soundEffects.Length)
        {
            return soundEffects[index];
        }
        else
        {
            Debug.LogError("Index out of range for sound effects.");
            return null;
        }
    }
}
