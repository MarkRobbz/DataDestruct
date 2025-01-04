using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointGizmo : MonoBehaviour
{
    public float gizmoLineLength = 2f;
    public Color gizmoColor = Color.green;
    public float arrowHeadLength = 0.25f;
    public float arrowHeadAngle = 20.0f;
    public GameObject taxiPrefabHeightCalulation; // Reference taxi prefab for height calculation

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;

        // line indicating forward direction
        Vector3 direction = transform.TransformDirection(Vector3.forward) * gizmoLineLength;
        Gizmos.DrawRay(transform.position, direction);

        // Arrowheads
        DrawArrowEnd(transform.position + direction, -direction, arrowHeadLength, arrowHeadAngle);

        //raycast to hit point (ground check)
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -Vector3.up, out hit))
        {
            //line from spawpoint to the hit point
            Gizmos.DrawLine(transform.position, hit.point);

            //sphere for hit point on the ground
            Gizmos.DrawSphere(hit.point, 0.1f); 

            
            Vector3 offsetPoint = hit.point + Vector3.up * (taxiPrefabHeightCalulation.GetComponent<Collider>().bounds.size.y * 0.5f);
            Gizmos.DrawLine(hit.point, offsetPoint);
            Gizmos.DrawSphere(offsetPoint, 0.1f); // Sphere to show the final spawn height
        }
    }

    void DrawArrowEnd(Vector3 position, Vector3 direction, float arrowHeadLength, float arrowHeadAngle)
    {
        Vector3 right = Quaternion.LookRotation(direction) * Quaternion.Euler(0, arrowHeadAngle, 0) * Vector3.forward * arrowHeadLength;
        Vector3 left = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -arrowHeadAngle, 0) * Vector3.forward * arrowHeadLength;
        Gizmos.DrawRay(position, right);
        Gizmos.DrawRay(position, left);
    }
}

