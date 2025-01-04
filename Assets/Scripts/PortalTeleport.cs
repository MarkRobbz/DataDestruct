using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalTeleporter : MonoBehaviour
{
    public Transform player;
    public Transform receiver;
    private PlayerCameraReset PlayerCamera;

    void Start()
    {
        // Get the CameraResetOnTeleport component from the player
        PlayerCamera = player.GetComponent<PlayerCameraReset>();
        if (PlayerCamera == null)
        {
            Debug.LogError("CameraResetOnTeleport component not found on player!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the portal trigger. Teleporting now.");
            TeleportPlayer();
        }
    }

    private void TeleportPlayer()
    {
        // Calculate the rotation difference
        float rotationDiff = -Quaternion.Angle(transform.rotation, receiver.rotation);
        rotationDiff += 180;
        player.Rotate(Vector3.up, rotationDiff);

        // Calculate the position offset and teleport the player
        Vector3 positionOffset = Quaternion.Euler(0f, rotationDiff, 0f) * (player.position - transform.position);
        player.position = receiver.position + positionOffset;

        // Use the separate script to reset the camera position
        if (PlayerCamera != null)
            PlayerCamera.ResetCameraPosition();
        else
            Debug.LogError("PlayerCamera component is not available to reset the camera.");
    }
}