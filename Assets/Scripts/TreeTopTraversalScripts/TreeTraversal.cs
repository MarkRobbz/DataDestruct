using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class TreeTraversal : MonoBehaviour
{
    public TreeNode rootNode;
    public TextMeshPro traversalOrderTextMesh;
    public TextMeshProUGUI interactionTextUI;

    private List<TreeNode> inorderList = new List<TreeNode>();
    private List<TreeNode> preorderList = new List<TreeNode>();
    private List<TreeNode> postorderList = new List<TreeNode>();
    private TreeNode currentNode;
    private int traversalIndex;

    public GameObject[] branches;
    public GameObject[] nodes;
    //Postions and Rotations of Branches/Nodes
    private Vector3[] originalBranchPositions;
    private Quaternion[] originalBranchRotations;
    private Vector3[] originalNodePositions;
    private Quaternion[] originalNodeRotations;

    public enum TraversalType { None, Inorder, Preorder, Postorder }
    public TraversalType currentTraversalType;

    private void Start()
    {
        BuildTraversalLists();
        currentTraversalType = TraversalType.None;
        Debug.Log($"Initial traversal type: {currentTraversalType}");
        // Cache the original positions and rotations of branches and nodes
        originalBranchPositions = new Vector3[branches.Length];
        originalBranchRotations = new Quaternion[branches.Length];
        for (int i = 0; i < branches.Length; i++)
        {
            if (branches[i] == null) continue;
            originalBranchPositions[i] = branches[i].transform.position;
            originalBranchRotations[i] = branches[i].transform.rotation;
        }

        originalNodePositions = new Vector3[nodes.Length];
        originalNodeRotations = new Quaternion[nodes.Length];
        for (int i = 0; i < nodes.Length; i++)
        {
            if (nodes[i] == null) continue;
            originalNodePositions[i] = nodes[i].transform.position;
            originalNodeRotations[i] = nodes[i].transform.rotation;
        }
    }


    public void SetTraversalType(TraversalType traversalType)
    {
        if (currentTraversalType == traversalType) return;

        currentTraversalType = traversalType;
        ResetTraversal();
        UpdateTraversalOrderTextMesh();
        StopAllCoroutines(); // Stop the current highlighting coroutine
        ResetMiniatureNodeHighlights(); // Reset the highlights immediately
        BuildTraversalLists();
        StartCoroutine(MiniatureNodeHighlight()); // Start a new highlighting coroutine
    }

    private void UpdateTraversalOrderTextMesh()
    {
        traversalOrderTextMesh.text = "Traversal Order: " + string.Join(", ", GetTraversalList().ConvertAll(node => node.nodeValue.ToString()));
    }

    private void ResetTraversal()
    {
        traversalIndex = 0;
        currentNode = GetFirstNodeInTraversal();
    }

    private void ResetAllNodeColors()
    {
        var allNodes = new HashSet<TreeNode>(inorderList);
        allNodes.UnionWith(preorderList);
        allNodes.UnionWith(postorderList);

        foreach (var node in allNodes)
        {
            ChangeNodeColor(node, Color.white);
        }
    }

    private TreeNode GetFirstNodeInTraversal()
    {
        return currentTraversalType switch
        {
            TraversalType.Inorder => inorderList.FirstOrDefault(),
            TraversalType.Preorder => preorderList.FirstOrDefault(),
            TraversalType.Postorder => postorderList.FirstOrDefault(),
            _ => null,
        };
    }

    public void NodeReached(TreeNode node)
    {
        var traversalList = GetTraversalList();
        if (traversalList == null || traversalList.Count == 0) return;

        // Check if the node is correct
        if (currentNode == node)
        {
            ChangeNodeColor(node, Color.green); // Correct node
            traversalIndex++;

            // Set the next current node or handle success if traversal is complete
            currentNode = traversalIndex < traversalList.Count ? traversalList[traversalIndex] : null;
            if (currentNode == null)
            {
                HandleSuccess();
            }
            else
            {
                ChangeNodeColor(currentNode, Color.yellow); // Next node to jump on
            }
        }
        else
        {
            ChangeNodeColor(node, Color.red); // Incorrect node
            HandleFailure();
            DropBranchesAndNodes();
        }
    }


    private void ChangeNodeColor(TreeNode node, Color color)
    {
        var renderer = node.GetComponent<Renderer>();
        if (renderer != null) renderer.material.color = color;
    }

    private List<TreeNode> GetTraversalList() => currentTraversalType switch
    {
        TraversalType.Inorder => inorderList,
        TraversalType.Preorder => preorderList,
        TraversalType.Postorder => postorderList,
        _ => null,
    };

    private void BuildTraversalLists()
    {
        inorderList.Clear();
        preorderList.Clear();
        postorderList.Clear();

        InorderTraversal(rootNode);
        PreorderTraversal(rootNode);
        PostorderTraversal(rootNode);
    }

    private void InorderTraversal(TreeNode node)
    {
        if (node == null) return;
        InorderTraversal(node.left);
        inorderList.Add(node);
        Debug.Log($"Added {node.name} to inorderList."); // Debugging output
        InorderTraversal(node.right);
    }

    private void PreorderTraversal(TreeNode node)
    {
        if (node == null) return;
        preorderList.Add(node);
        PreorderTraversal(node.left);
        PreorderTraversal(node.right);
    }

    private void PostorderTraversal(TreeNode node)
    {
        if (node == null) return;
        PostorderTraversal(node.left);
        PostorderTraversal(node.right);
        postorderList.Add(node);
    }

    public bool IsCorrectNode(TreeNode node)
    {
        return GetTraversalList()?.ElementAtOrDefault(traversalIndex) == node;
    }

    private IEnumerator ResetTraversal(float delay)
    {
        yield return new WaitForSeconds(delay);
        ResetTraversal();
        ResetAllNodeColors();
    }

    private void HandleSuccess()
    {
        traversalOrderTextMesh.text = "Traversal Complete!";
        GameManager.instance.CompleteTaskInSection("TreeTopTraversal");
    }



    private void HandleFailure()
    {
        
        if (interactionTextUI != null)
        {
            interactionTextUI.gameObject.SetActive(true);
            interactionTextUI.text = "Try again!";
            StartCoroutine(ClearInteractionTextAfterDelay(4f)); //hide text
        }
        else
        {
            Debug.LogError("InteractionTextUI is not assigned!");
        }

        
        StartCoroutine(ResetTraversal(1f));
    }

    private IEnumerator ClearInteractionTextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Cleartext
        if (interactionTextUI != null)
        {
            interactionTextUI.text = "";
            interactionTextUI.gameObject.SetActive(false);
        }
    }


    private IEnumerator MiniatureNodeHighlight()
    {
        Debug.Log($"Highlighting started for traversal type: {currentTraversalType}"); // Confirm coroutine is started
        while (true)
        {
            var traversalList = GetTraversalList();
            foreach (var node in traversalList)
            {
                if (node.miniatureRepresentation != null)
                {
                    var renderer = node.miniatureRepresentation.GetComponent<Renderer>();
                    renderer.material.color = Color.yellow; // Highlight next node
                    yield return new WaitForSeconds(1f); // Delay color change
                    renderer.material.color = Color.green; // Visited node
                    Debug.Log($"Highlighted node: {node.name}"); // Confirm each node is highlighted
                }
                else
                {
                    Debug.LogError($"Node {node.name} has no miniature representation assigned.");
                }
            }
            yield return new WaitForSeconds(0.5f);
            ResetAllNodeColors();
            yield return new WaitForSeconds(1f); // Pause before starting the cycle again
        }
        
    }

    private void ResetMiniatureNodeHighlights()
    {
        foreach (var nodeGameObject in nodes)
        {
            TreeNode node = nodeGameObject.GetComponent<TreeNode>();
            if (node != null && node.miniatureRepresentation != null)
            {
                Renderer renderer = node.miniatureRepresentation.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = Color.white; // Reset to default color
                }
            }
        }
    }



    private void DropBranchesAndNodes()
    {
        foreach (var branch in branches)
        {
            var rb = branch.GetComponent<Rigidbody>();
            if (rb == null) continue;
            rb.isKinematic = false;
            rb.useGravity = true;
        }
        foreach (var node in nodes)
        {
            var rb = node.GetComponent<Rigidbody>();
            if (rb == null) continue;
            rb.isKinematic = false;
            rb.useGravity = true;
        }

        // Start coroutine to reset after delay
        StartCoroutine(ResetBranchesAndNodes(5f));
    }

    private IEnumerator ResetBranchesAndNodes(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Reset branches to their initial positions and disable physics
        for (int i = 0; i < branches.Length; i++)
        {
            Rigidbody rb = branches[i].GetComponent<Rigidbody>();
            if (rb == null) continue;
            rb.isKinematic = true;
            rb.useGravity = false;
            branches[i].transform.position = originalBranchPositions[i];
            branches[i].transform.rotation = originalBranchRotations[i];
        }

        // Reset nodes orignal positions
        for (int i = 0; i < nodes.Length; i++)
        {
            Rigidbody rb = nodes[i].GetComponent<Rigidbody>();
            if (rb == null) continue;
            rb.isKinematic = true;
            rb.useGravity = false;
            nodes[i].transform.position = originalNodePositions[i];
            nodes[i].transform.rotation = originalNodeRotations[i];
        }
        ResetTraversal();
    }
}
