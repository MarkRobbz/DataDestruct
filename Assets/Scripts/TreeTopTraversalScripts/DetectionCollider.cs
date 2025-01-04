using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// *Attach script to collider of each TreeNode*
public class DetectionCollider : MonoBehaviour
{
    private TreeNode parentNode;

    private void Start()
    {
        // DetectionCollider is child of the TreeNode GameObject
        parentNode = GetComponentInParent<TreeNode>();
        if (parentNode == null)
        {
            Debug.LogError("DetectionCollider could not find TreeNode component in parent.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (parentNode != null)
            {
                parentNode.NotifyNodeReached();
            }
            else
            {
                Debug.LogError("parentNode is not set on " + gameObject.name);
            }
        }
    }
}

