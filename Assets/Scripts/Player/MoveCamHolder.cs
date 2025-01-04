using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCamHolder : MonoBehaviour
{
    public Transform cameraPosition;

    void Update()
    {
        transform.position = cameraPosition.position;
    }

   
}


