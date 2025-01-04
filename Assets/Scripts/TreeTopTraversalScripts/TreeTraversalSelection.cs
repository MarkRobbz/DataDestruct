using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TreeTraversalSelection : MonoBehaviour
{
    public TextMeshProUGUI interactionTextUI;
    public TreeTraversal traversalScript;
    public TreeTraversal.TraversalType traversalType;
    private bool isPlayerClose = false;

    public TraversalMethodAutoSelector AutoCollider;

    private void Update()
    {
        if (isPlayerClose && Input.GetKeyDown(KeyCode.E))
        {
            traversalScript.SetTraversalType(traversalType);
            // Display a confirmation message for the selected traversal type
            interactionTextUI.text = $"Selected: {traversalType}";
            AutoCollider.gameObject.SetActive(false); // Hiding auto selector collider
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerClose = true;
            // Set the interaction text to indicate the current state
            string message = traversalScript.currentTraversalType == traversalType
                ? $"Traversal Method Selected: {traversalType}"
                : $"Press E to select {traversalType}";
            interactionTextUI.text = message;
            interactionTextUI.gameObject.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerClose = false;
            // Clear text when the player is no longer close
            interactionTextUI.text = "";
            interactionTextUI.gameObject.SetActive(false);
        }
    }
}
