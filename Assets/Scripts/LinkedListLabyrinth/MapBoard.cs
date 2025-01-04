using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapBoard : MonoBehaviour
{
    public GameObject[] routes; // Paths through the maze
    public GameObject[] mapNodes;
    public NodeInteraction lastInteractedNode;// track the last interacted node
    

    public void SetStartingNode(int nodeIndex, NodeInteraction nodeInteraction)
    {
       // Debug.Log("SetStartingNode called with nodeIndex: " + nodeIndex); 
        lastInteractedNode = nodeInteraction;
        HighlightPathToNode(nodeInteraction.nextNode);
    }

    public void HighlightPathToNode(NodeInteraction targetNode)
    {
        foreach (var route in routes)
        {
            route.SetActive(false);
        }

        // activate route that leads to the target node
        if (targetNode != null && targetNode.nodeIndex < routes.Length)
        {
            routes[targetNode.nodeIndex].SetActive(true);
        }

        if (lastInteractedNode != null)
        {
            lastInteractedNode.ResetInteraction(); 
            lastInteractedNode = null; // Clear reference
        }
    }

    public void DeactivateAllRoutes()
    {
        foreach (var route in routes)
        {
            route.SetActive(false);
        }
    }



    public void ChangeMapNodeColour(int nodeIndex, Material newMaterial)
    {
        //Debug.Log("Attempting to change color for node at index: " + nodeIndex); 
        if (nodeIndex >= 0 && nodeIndex < mapNodes.Length)
        {
            var mapNodeRenderer = mapNodes[nodeIndex].GetComponent<MeshRenderer>();
            if (mapNodeRenderer != null)
            {
                //Debug.Log("Changing color of map node: " + mapNodes[nodeIndex].name); 
                mapNodeRenderer.material = newMaterial; 
            }
            else
            {
                Debug.LogError("MeshRenderer not found on map node: " + mapNodes[nodeIndex].name); 
            }
        }
        else
        {
            Debug.LogError("Invalid node index: " + nodeIndex); 
        }
    }
}

