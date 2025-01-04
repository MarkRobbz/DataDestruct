using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuizTrigger : MonoBehaviour
{
    public string interactionText = "";
    public GameObject interactionUI; // UI Element for text to appear on screen
    public QuizManager quizManager;
    public int quizDatasetIndex;
    public List<Key> endDoorKeys = new List<Key>(); // Quiz can now sue mutiple keys
    private bool isPlayerInRange = false;

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            interactionUI.SetActive(false);
            GameManager.instance.ShowQuiz();
            quizManager.InitializeQuiz(quizDatasetIndex, this); // Use the specific index for this trigger

            GameManager.instance.UnlockCursor();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TextMeshProUGUI textComponent = interactionUI.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = interactionText;
            }

            interactionUI.SetActive(true);
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TextMeshProUGUI textComponent = interactionUI.GetComponent<TextMeshProUGUI>();
            if (textComponent != null)
            {
                textComponent.text = "";
            }
            interactionUI.SetActive(false);
            isPlayerInRange = false;
            GameManager.instance.LockCursor();
        }
    }

    public void RevealKey()
    {
        if (endDoorKeys != null && endDoorKeys.Count > 0)
        {
            foreach (Key key in endDoorKeys)
            {
                if (key != null)
                {
                    key.gameObject.SetActive(true);
                }
                else
                {
                    Debug.LogError("One of the Key objects in the list is not assigned or is null.");
                }
            }
        }
        else
        {
            Debug.LogError("Key objects list is not assigned or is empty.");
        }
    }
}
