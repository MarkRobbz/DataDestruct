using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro; // Include the namespace for TextMeshPro

public class TraversalMethodAutoSelector : MonoBehaviour
{
    public TreeTraversal traversalScript;
    public TextMeshProUGUI interactionTextUI; 
    private TreeTraversal.TraversalType[] traversalTypes =
    {
        TreeTraversal.TraversalType.Inorder,
        TreeTraversal.TraversalType.Preorder,
        TreeTraversal.TraversalType.Postorder
    };

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Select a random traversal type
            TreeTraversal.TraversalType randomType = traversalTypes[Random.Range(0, traversalTypes.Length)];
            traversalScript.SetTraversalType(randomType);
            // Update the UI to 
            interactionTextUI.text = "*Automatic* Traversal Method Selected: " + randomType;
            interactionTextUI.gameObject.SetActive(true);
            
            StartCoroutine(HideTextAfterDelay(5.0f)); // Hides text after 5 seconds
        }
    }

    private IEnumerator HideTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        interactionTextUI.gameObject.SetActive(false);
    }
}