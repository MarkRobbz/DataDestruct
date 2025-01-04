using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaxiSpawnerToggle : MonoBehaviour
{
    public GameObject taxiSpawnParent; // Object that contains Taxi Spawnpoints
    private bool isSpawnerActive = false; 

    private void OnTriggerEnter(Collider other)
    {
        // Check if the collider is a player
        if (other.CompareTag("Player"))
        {
            // Toggle of TaxiPsawn points
            isSpawnerActive = !isSpawnerActive;
            taxiSpawnParent.SetActive(isSpawnerActive);
        }
    }
}
