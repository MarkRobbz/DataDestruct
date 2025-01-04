using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class QuitGame : MonoBehaviour
{
    public Button mainMenuButton;
    public Button quitButton;

    private GameSceneManager gameSceneManager;

    private void Awake()
    {
        // Find the GameSceneManager instance when this script is initialized
        gameSceneManager = FindObjectOfType<GameSceneManager>();

        if (gameSceneManager == null)
        {
            Debug.LogError("UIManager: GameSceneManager not found in the scene.");
            return;
        }

        // Add listeners to the buttons to call the appropriate methods on click
        if (mainMenuButton != null)
            mainMenuButton.onClick.AddListener(gameSceneManager.LoadMainMenu);
        if (quitButton != null)
            quitButton.onClick.AddListener(gameSceneManager.QuitGame);
    }
}
