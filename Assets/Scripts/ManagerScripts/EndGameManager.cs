using UnityEngine;
using UnityEngine.UI;

public class EndGameManager : MonoBehaviour
{
    [SerializeField] private Button mainMenuButton;
    [SerializeField] private Button quitButton;
    private GameSceneManager gameSceneManager;

    private void Start()
    {
        // Find the GameSceneManager component in the current scene
        gameSceneManager = FindObjectOfType<GameSceneManager>();

        if (gameSceneManager != null)
        {
            // Ensure your buttons are assigned, either via inspector or by finding them here
            if (mainMenuButton == null || quitButton == null)
            {
                Debug.LogError("EndGameManager: Buttons are not assigned.");
                return;
            }

            mainMenuButton.onClick.AddListener(gameSceneManager.LoadMainMenu);
            quitButton.onClick.AddListener(gameSceneManager.QuitGame);
        }
        else
        {
            Debug.LogError("EndGameManager: GameSceneManager instance not found.");
        }
    }
}