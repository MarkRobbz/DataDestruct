using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitDoorTrigger : MonoBehaviour
{
    public string targetSceneName = "GameScene"; // The name of the scene to load

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Make sure the collider belongs to the player
        {
            LoadTargetScene();
        }
    }

    private void LoadTargetScene()
    {
        SceneManager.LoadScene(targetSceneName);
    }
}


