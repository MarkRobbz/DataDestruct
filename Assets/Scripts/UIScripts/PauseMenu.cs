using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    private GameManager gameManager;
    public PlayerRespawn respawnObj;
    private GameSceneManager sceneManager;
    bool isPaused;

    private void Start()
    {
        // Ensures PauseMenu is hidden at the start
        // pauseMenu.SetActive(false);    *Disabled it in editor instead*
        gameManager = FindObjectOfType<GameManager>();
        respawnObj = FindObjectOfType<PlayerRespawn>();
        sceneManager = FindObjectOfType<GameSceneManager>();

        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
        if (respawnObj == null)
        {
            respawnObj = FindAnyObjectByType<PlayerRespawn>();
        }
        if (sceneManager == null)
        {
            sceneManager = FindObjectOfType<GameSceneManager>();
        }
    }



    public void TogglePauseMenu()
    {
        isPaused = !isPaused;
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(isPaused);
            Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isPaused;
        }
        else
        {
            Debug.LogError("PauseMenu GameObject is not assigned.");
        }
        Time.timeScale = isPaused ? 0 : 1;
    }


    public void MainMenu()
    {
        Debug.Log("Main menu button clicked!");

        sceneManager.LoadMainMenu();
    }

    //public void Resume()
    //{

    //}

    public void Respawn()
    {
        
        respawnObj.Respawn();
        ClosePauseMenu();
        
    }
    public void ClosePauseMenu()
    {
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Time.timeScale = 1; // resumed
            isPaused = false; 
        }
        else
        {
            Debug.LogError("PauseMenu GameObject is not assigned.");
        }
    }

    //public void openSettigns()
    //{

    //}

    public void QuitGame()
    {
        Debug.Log("Quit Game button clicked");
        sceneManager.QuitGame();
    }

    private void Paused()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;
        pauseMenu.SetActive(isPaused);
    }
}
