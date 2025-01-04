using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerCameraReset : MonoBehaviour
{
    public CinemachineFreeLook cinemachineFreeLook;

    private void Reset()
    {
        // Attempt to auto-find the CinemachineFreeLook component
        if (cinemachineFreeLook == null)
        {
            cinemachineFreeLook = FindObjectOfType<CinemachineFreeLook>();
        }
    }

    public void ResetCameraPosition()
    {
        if (cinemachineFreeLook != null)
        {
            cinemachineFreeLook.PreviousStateIsValid = false; // Reset the state
            cinemachineFreeLook.UpdateCameraState(transform.position, Time.deltaTime); // Force an update
        }
    }
}

