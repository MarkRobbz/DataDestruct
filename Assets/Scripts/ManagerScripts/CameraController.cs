using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineBrain mainCameraBrain; 
    public CinemachineFreeLook playerFreeLookCamera; // main camera
    public CinemachineVirtualCamera[] cinematicCameras; 

    private void Start()
    {
        // Ensure the main free-look came has highest priority on start
        playerFreeLookCamera.Priority = 10;
    }


    public void SwitchToCamera(CinemachineVirtualCamera cinematicCamera)
    {
        if (cinematicCamera != null)
        {
            mainCameraBrain.m_DefaultBlend.m_Time = 0; // Immediate cut

            // Set the cinematic camera to a higher priority to make it active
            cinematicCamera.Priority = playerFreeLookCamera.Priority + 1;

            Debug.Log("Switched to camera: " + cinematicCamera.name);
        }
        else
        {
            Debug.LogError("Cinematic camera is null.");
        }
    }

    public void SwitchBackToMainCamera() //*Make sure to assign Camera to Camera Controller and Gamemanager section script*
    {
        // Set the main camera back to its original priority
        playerFreeLookCamera.Priority = 10;

        // Reset all other cameras to a lower priority
        foreach (var cam in cinematicCameras)
        {
            cam.Priority = 0; // Assuming 0 is the default non-active state
        }

        Debug.Log("Switched back to the player camera.");
    }


    public void MoveCamera(CinemachineVirtualCamera cinematicCamera, Vector3 targetPosition, float duration)
    {
        StartCoroutine(AnimatePosition(cinematicCamera, targetPosition, duration));
    }

    private IEnumerator AnimatePosition(CinemachineVirtualCamera cam, Vector3 targetPosition, float duration)
    {
        Vector3 startPosition = cam.transform.localPosition;
        float time = 0;

        while (time < duration)
        {
            cam.transform.localPosition = Vector3.Lerp(startPosition, targetPosition, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        cam.transform.localPosition = targetPosition;
    }

    public void AnimateCamera(CinemachineVirtualCamera cinematicCamera, AnimationParameters parameters)
    {
        if (parameters.usePositionAnimation) //Moves camera
        {
            StartCoroutine(AnimatePosition(cinematicCamera, parameters.targetPosition, parameters.animationDuration));
        }
        else
        {
            //Zooms fov in instead
            StartCoroutine(AnimateFOV(cinematicCamera, parameters.targetFOV, parameters.animationDuration));
        }
    }

    private IEnumerator AnimateFOV(CinemachineVirtualCamera cam, float targetFOV, float duration)
    {
        float startFOV = cam.m_Lens.FieldOfView; // Get starting FOV from the cam lens
        float time = 0f;

        while (time < duration)
        {
            // Lerp  FOV from original value to the target value over specified duration
            cam.m_Lens.FieldOfView = Mathf.Lerp(startFOV, targetFOV, time / duration);
            time += Time.deltaTime;
            yield return null;
        }

        // Ensure final FOV is set to targt val
        cam.m_Lens.FieldOfView = targetFOV;
    }



}

