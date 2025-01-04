using UnityEngine;

public class EndGame : MonoBehaviour
{
    private GameSceneManager sceneManager;

    private void Start()
    {
        sceneManager = FindObjectOfType<GameSceneManager>();

        if (sceneManager == null)
        {
            Debug.LogError("EndGame: No GameSceneManager found in the scene. Please make sure there is a GameSceneManager object with the GameSceneManager script attached to it.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (sceneManager != null)
            {
                sceneManager.LoadEndGameScene();
            }
            else
            {
                Debug.LogError("EndGame: GameSceneManager is null. The LoadEndGameScene method couldn't be called!");
            }
        }
    }
}
