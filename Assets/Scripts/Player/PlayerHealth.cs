using System;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 4;
    public int currentHealth;
    public PlayerRespawn respawnPlayer;

    public static event Action OnPlayerDamaged;

    private void Start()
    {
        currentHealth = maxHealth;
        respawnPlayer = GetComponent<PlayerRespawn>(); // Make sure the PlayerRespawn component is on the same GameObject
        OnPlayerDamaged?.Invoke();
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        OnPlayerDamaged?.Invoke();

        if (currentHealth <= 0)
        {
            Respawn();
        }
    }

    public void GiveHealth(int amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
        OnPlayerDamaged?.Invoke();
    }

    private void Respawn()
    {
        respawnPlayer.Respawn(); 
        currentHealth = maxHealth; // Reset health
        OnPlayerDamaged?.Invoke(); // Update UI
        Debug.Log("Player respawned with full health.");
    }

    public void RespawnedViaMenu()
    {
        currentHealth = maxHealth; 
        Debug.Log("Player respawned with full health after clikcing Respawn button.");
    }
}