using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisibilityTrigger : MonoBehaviour
{
    public GameObject[] objectsToShowOrHide; 

    private void SetActiveObjects(bool active)
    {
        foreach (GameObject obj in objectsToShowOrHide)
        {
            if (obj != null) // Check to avoid null reference
            {
                obj.SetActive(active);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check player has entered trigger collider
        if (other.CompareTag("Player"))
        {
            SetActiveObjects(true); // Show all objects
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check player has exited trigger collider
        if (other.CompareTag("Player"))
        {
            SetActiveObjects(false); // Hide all objects
        }
    }
}

