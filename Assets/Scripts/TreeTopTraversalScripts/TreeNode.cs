using UnityEngine;

public class TreeNode : MonoBehaviour
{
    public int nodeValue;
    public TreeNode left;
    public TreeNode right;

    public TreeTraversal treeTraversal;

    public GameObject miniatureRepresentation;


    public void NotifyNodeReached()
    {
        if (treeTraversal != null)
        {
            treeTraversal.NodeReached(this);
        }
        else
        {
            Debug.LogError("treeTraversal is not set on " + gameObject.name);
        }
    }
}


