using UnityEngine;
using Cinemachine;

public class BillBoard : MonoBehaviour
{
    //Add this script to a textMeshPro Object
    private Transform mainCameraTransform;

    void Start()
    {
        //Grabs the main camera transform.
        mainCameraTransform = Camera.main.transform;
    }

    void Update()
    {
        if (mainCameraTransform != null)
        {
            // Use to make sure text is not backwards
            Vector3 directionFromCamera = transform.position - mainCameraTransform.position;

            // keeps the text aligned horizontally (Wont make it tilt etc)
            directionFromCamera.y = 0;

            // Create the rotation from the direction (follows the player)
            Quaternion lookRotation = Quaternion.LookRotation(directionFromCamera);

            //Applies to face the camera
            transform.rotation = lookRotation;
        }
    }
}
