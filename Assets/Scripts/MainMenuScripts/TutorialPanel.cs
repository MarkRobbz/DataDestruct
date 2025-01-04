using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialPanel : MonoBehaviour
{
    public GameObject btnPanelTutorial;// Assign this in the inspector
    public GameObject btnPanelTutorialClose;
    
    // Call this method when the Play button is clicked
    public void ToggleTutorialPanel()
    {
        // This will toggle the active state of the tutorial panel
        btnPanelTutorial.SetActive(!btnPanelTutorial.activeSelf);
        this.gameObject.SetActive(!this.gameObject.activeSelf);
    }
}
