using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfBoundsTrigger : MonoBehaviour
{
    public PlayerRespawn playerRespawn;  // Reference to the PlayerRespawn script.

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Call Respawn method in PlayerRespawn script when player enters the trigger.
            playerRespawn.Respawn();
        }
    }
}