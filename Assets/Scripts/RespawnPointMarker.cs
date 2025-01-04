using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class RespawnPointMarker : MonoBehaviour
{
    [SerializeField]
    private int raycastLayerToIgnore = 0; //player layer is 8
    private int layerAsMask;

   
    void OnDrawGizmos()
    {
        RaycastHit hit;
        layerAsMask = (1 << raycastLayerToIgnore); //Left-shift expression for getting the layer level
        layerAsMask = ~layerAsMask; //Ignore this layer


        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 5f,layerAsMask))
        {
            Debug.DrawLine(transform.position, hit.point, Color.yellow);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(hit.point, 2.5f);
        }
    }
}
