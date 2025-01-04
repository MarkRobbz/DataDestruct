using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NodeInteraction : MonoBehaviour
{
    public float interactionRange = 5f;
    public KeyCode interactionKey = KeyCode.E;
    public GameObject player;
    public int nodeIndex;
    public NodeInteraction nextNode;
    public TextMeshProUGUI interactionTextUI; 
    public GameObject nodeLight; 
    public Material activatedMaterial; 

    private bool isInteractedWith = false;
    public GameObject[] possibleRoutesToNextNode; // Possible routes that this node can reveal

    private static NodeInteraction currentActiveNode; // Track currently active node

    private void Start()
    {
        // Ensure the interaction text is not visible at the start
        if (interactionTextUI != null)
        {
            interactionTextUI.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (!isInteractedWith)
        {
            bool playerClose = IsPlayerClose();
            if (playerClose)
            {
                
                if (currentActiveNode == null || currentActiveNode == this)
                {
                    currentActiveNode = this; // This node is now the active one
                    interactionTextUI.gameObject.SetActive(true);
                    interactionTextUI.text = "Press E to activate node";
                }
            }
            else if (currentActiveNode == this)
            {
                // Hide interaction text and clear current active node (if not close enough)
                currentActiveNode = null;
            }

            if (playerClose && Input.GetKeyDown(interactionKey))
            {
                ActivateNode();
            }
        }
    }

    private bool IsPlayerClose()
    {
        // Check if the player is within the interaction range of the node
        return Vector3.Distance(player.transform.position, transform.position) <= interactionRange;
    }

    private void ActivateNode()
    {
        Debug.Log($"Interacted with node index: {nodeIndex}");
        isInteractedWith = true;

        // Change activated material
        if (nodeLight != null)
        {
            Renderer renderer = nodeLight.GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = activatedMaterial;
            }
            else
            {
                Debug.LogError("Renderer not found on the nodeLight object");
            }
        }

        // Update the maps with current node interaction
        GameManager.instance.UpdateAllMaps(this, nodeIndex);

        // Hide interaction text after 
        if (interactionTextUI != null)
        {
            interactionTextUI.gameObject.SetActive(false);// Hide the interaction text after interacting
        }

        // Clear current active node
        currentActiveNode = null;
    }

    public void ResetInteraction()
    {
        // Reset node to be interactable again
        isInteractedWith = false;

      
    }
}
