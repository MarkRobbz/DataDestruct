using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    private Vector3 initialPosition; // Store the starting position
    private Transform lastRespawnPoint; // Store the last respawn point
    private PlayerHealth playerHealth;
    private AudioManager audioManager;

    private void Start()
    {
        initialPosition = transform.position; // Save the initial position as the default respawn location
        playerHealth = GetComponent<PlayerHealth>(); // Assign the PlayerHealth component
        audioManager = FindObjectOfType<AudioManager>(); // Find the AudioManager in the scene
        if (audioManager == null)
        {
            Debug.LogError("PlayerRespawn: No AudioManager found in the scene.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerRespawnPoint"))
        {
            lastRespawnPoint = other.transform; // Update the last respawn point
            Debug.Log("Respawn point set: " + lastRespawnPoint.name);
        }
    }

    public void Respawn()
    {
        transform.position = lastRespawnPoint != null ? lastRespawnPoint.position : initialPosition;
        // Ensure that playerHealth is assigned before calling methods on it
        if (playerHealth == null)
        {
            Debug.LogError("PlayerRespawn: PlayerHealth component not found.");
            return;
        }

        if (audioManager != null)
        {
            // Play the respawn sound effect
            AudioClip clipToPlay = audioManager.GetSoundEffect(2); // Use the method directly from AudioManager
            if (clipToPlay != null)
            {
                audioManager.PlaySFX(clipToPlay);
            }
            else
            {
                Debug.LogError("PlayerRespawn: Respawn sound effect clip not found.");
            }
        }
        else
        {
            Debug.LogError("PlayerRespawn: AudioManager is not configured properly.");
        }

        playerHealth.RespawnedViaMenu();
    }
}
