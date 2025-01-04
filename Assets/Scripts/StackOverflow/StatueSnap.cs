using UnityEngine;
using System.Collections.Generic;

public class StatueSnap : MonoBehaviour
{
    [SerializeField] private Transform snapPoint;
    [SerializeField] private GameObject completeStatue;
    [SerializeField] private GameObject[] statueParts;
    [SerializeField] private GameObject arrowsParent; // Parent GameObject of the arrows
    public float snapRange = 1f;
    private bool isSnapped = false;
    private static List<StatueSnap> snappedPieces = new List<StatueSnap>();

    public float manualHeightAdjustment = 0f;

    void FixedUpdate()
    {
        if (!isSnapped && snapPoint != null && Vector3.Distance(transform.position, snapPoint.position) <= snapRange)
        {
            SnapToPlace();
        }
    }

    private void SnapToPlace()
    {
        //Debug.Log(gameObject.name + " is snapping to place.");

        transform.position = snapPoint.position;
        transform.rotation = Quaternion.identity;
        isSnapped = true;

        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)  //Locks postion
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        
        if (gameObject.name == "VikingStatueBottom")
        {
            // Activate the complete statue 
            if (completeStatue != null)
            {
                completeStatue.SetActive(true);
                // Debug.Log("Complete statue activated.");
                GameManager.instance.CompleteTaskInSection("StackOverflow");
            }
            else
            {
                Debug.LogError("Complete statue not assigned.");
            }

            // Hide all the individual parts
            foreach (var part in statueParts)
            {
               // Debug.Log("Deactivating part: " + part.gameObject.name);
                part.SetActive(false);
            }

            // Hide the arrows parent
            if (arrowsParent != null)
            {
                arrowsParent.SetActive(false);
                Debug.Log("Arrows hidden.");
            }

            // Clear the list of snapped pieces and disable the script
            snappedPieces.Clear();
            this.enabled = false;
        }
        else
        {
            // For any other piece, add it to the list and move up previously snapped pieces
            snappedPieces.Add(this);

            // Calculate height to move up based on the renderer bounds or manual adjustment
            float heightToMoveUp = manualHeightAdjustment > 0 ? manualHeightAdjustment : GetComponent<Renderer>().bounds.size.y; 
            foreach (var piece in snappedPieces)
            {
                if (piece != this)
                {
                    piece.transform.position += new Vector3(0, heightToMoveUp, 0);
                   // Debug.Log(piece.gameObject.name + " moved up by " + heightToMoveUp);
                }
            }
        }
    }

}
