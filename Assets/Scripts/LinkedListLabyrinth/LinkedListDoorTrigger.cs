using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LinkedListDoorTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] doors;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with: " + other.name);
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player tagged object entered the trigger.");
            OpenDoor();
        }
    }

    private void OpenDoor()
    {
        Debug.Log("OpenDoor method called.");
        foreach (var door in doors)
        {
            if (door != null)
            {
                Debug.Log("Checking door: " + door.name);
                Animator doorAnimator = door.GetComponent<Animator>();
                if (doorAnimator != null)
                {
                    Debug.Log("Animator found on door: " + door.name);
                    doorAnimator.SetBool("Open", true);
                    Debug.Log("Animator called for door: " + door.name);
                }
                else
                {
                    Debug.Log("Animator not found on door: " + door.name);
                }
            }
            else
            {
                Debug.Log("Door GameObject is null in the array.");
            }
        }
        Debug.Log("OpenDoor method finished.");
    }
}