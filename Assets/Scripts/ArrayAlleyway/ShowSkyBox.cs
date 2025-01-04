using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ShowSkyBox : MonoBehaviour
{
    [SerializeField]
    private GameObject skyBoxPrefab;

   

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            skyBoxPrefab.SetActive(true);
            
        }
        
    }

 
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            skyBoxPrefab.SetActive(false);
            
        }
    }
}