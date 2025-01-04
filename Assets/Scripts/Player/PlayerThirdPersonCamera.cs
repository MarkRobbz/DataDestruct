using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThirdPersonCam : MonoBehaviour
{
    [Header("References")]
    public Transform orientation; //To store data for direction player is facing.
    public Transform player;
    public Transform playerObject;
    public Rigidbody rigiBod;

    public float rotationSpeed;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void FixedUpdate()
    {
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (inputDir != Vector3.zero)
        {
            playerObject.forward = Vector3.Slerp(playerObject.forward, inputDir.normalized, Time.deltaTime * rotationSpeed);

        }
    }

    

}
