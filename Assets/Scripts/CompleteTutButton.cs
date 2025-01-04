using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompleteTutButton : MonoBehaviour
{
    
    [SerializeField] private GameObject objectToActivate;
    
    [SerializeField] private GameObject quizButton;
    
    public void ActivateObjects()
    {
        if (objectToActivate != null)
        {
            objectToActivate.SetActive(true);
            quizButton.SetActive((true));
        }
    }

   
}
