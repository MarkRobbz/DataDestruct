using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    // No static instance reference since we're not using a singleton.

    private void Awake()
    {
        // Subscribe to the sceneLoaded event.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe to avoid memory leaks when the scene manager is destroyed.
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    // This method is called every time a new scene is loaded.
   

    // Call these methods to load the respective scenes
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    public void LoadGameOverScene()
    {
        SceneManager.LoadScene("GameOverScene");
    }

    public void LoadEndGameScene()
    {
        SceneManager.LoadScene("EndGameScene");
    }

    public void LoadTutorialScene()
    {
        SceneManager.LoadScene("TutorialScene");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EnsureAudioManagerExists();

        if (scene.name == "GameScene")
        {
            Time.timeScale = 1;
        }
        // Additional scene-specific setup...
    }

    private void EnsureAudioManagerExists()
    {
        AudioManager existingAudioManager = FindObjectOfType<AudioManager>();
        if (existingAudioManager == null)
        {
            Debug.LogError("No AudioManager found in the newly loaded scene.");

            // Optionally instantiate a new AudioManager if you have a prefab ready.
            // AudioManagerPrefab is a reference to your AudioManager prefab.
            // You would set this in the inspector or find it via Resources.
            AudioManager audioManagerPrefab = Resources.Load<AudioManager>("Prefabs/AudioManager");
            if (audioManagerPrefab != null)
            {
                AudioManager newInstance = Instantiate(audioManagerPrefab);
                newInstance.transform.SetAsFirstSibling(); // Optional: to keep it organized at the top of the hierarchy.
            }
            else
            {
                Debug.LogError("No AudioManager prefab found. Make sure it's located in the Resources/Prefabs directory.");
            }
        }
        else
        {
            
        }
    }

    
}