using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnimation : MonoBehaviour
{
    private Vector3 originalPosition;

    void Start()
    {
        // Store the original position
        originalPosition = transform.position;
    }

   

    public void PlayDoorAnimation()
    {
        // Set the X and Z position to the original values before playing the animation
        Vector3 currentPosition = transform.position;
        transform.position = new Vector3(originalPosition.x, currentPosition.y, originalPosition.z);

        // Trigger your animation here
        GetComponent<Animator>().SetTrigger("PlayDoorAnimation");
    }
}
