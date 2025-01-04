using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;


public class StartButton : MonoBehaviour
{
    private GameSceneManager sceneManager;

    private void Start()
    {
        sceneManager = FindObjectOfType<GameSceneManager>();

        Button startButton = GetComponent<Button>();
        if (startButton != null)
        {
            startButton.onClick.AddListener(sceneManager.LoadGameScene);
        }
    }
}
//void TestButtonFunction()
//{
//    Debug.Log("Button was clicked!");
//}






