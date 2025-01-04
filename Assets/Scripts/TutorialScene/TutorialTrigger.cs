using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialTrigger : MonoBehaviour
{
    public GameObject tutorialsUI; // The parent container for all tutorial UIs
    public GameObject[] tutorialPages; // Array of tutorial UI pages related to this trigger
    public Button backButton;
    public Button nextButton;
    private TextMeshProUGUI nextButtonText; 
    private int currentPageIndex = 0;
    private bool hasBeenTriggered = false;

    void Start()
    {
        tutorialsUI.SetActive(false); // Ensure the tutorial UI is not visible initially
        DeactivateAllTutorialPages(); // Deactivate all tutorial pages
        nextButtonText = nextButton.GetComponentInChildren<TextMeshProUGUI>(); 
        backButton.onClick.AddListener(GoToPreviousPage);
        nextButton.onClick.AddListener(GoToNextPage);
    }

    private void Update()
    {
        CheckSpaceBarInput();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasBeenTriggered) // Check if the trigger hasn't been activated 
        {
            hasBeenTriggered = true; // Sets flag prevent reactivation
            currentPageIndex = 0; // Reset first page
            DeactivateAllTutorialPages(); // make sure no other pages are active
            tutorialsUI.SetActive(true);
            ShowPage(currentPageIndex); // Show the first page
            PauseGame();
        }
    }


    void ShowPage(int index)
    {
        // Hide all pages
        foreach (var page in tutorialPages)
        {
            page.SetActive(false);
        }

        // Show the current page
        if (index >= 0 && index < tutorialPages.Length)
        {
            tutorialPages[index].SetActive(true);
        }
        else
        {
            Debug.LogError("Index out of range for tutorial pages: " + index);
        }

        // Update the button states every time a page is shown
        UpdateButtonStates();
    }


    void GoToNextPage()
    {
        if (currentPageIndex < tutorialPages.Length - 1)
        {
            currentPageIndex++;
            ShowPage(currentPageIndex);
            Debug.Log("Going to next page: " + currentPageIndex);
        }
        else
        {
            ContinueGame();
            Debug.Log("ContinueGame called from GoToNextPage - this should only happen on the last page");
        }
    }


    void GoToPreviousPage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            ShowPage(currentPageIndex);
        }
    }

    void UpdateButtonStates()
    {
        // Enables back button if not on the first page
        backButton.interactable = currentPageIndex > 0;

        
        //Debug.Log("Current Page Index: " + currentPageIndex + " | Total Pages: " + tutorialPages.Length);

        // If last page, shows continue, sets button to call ContinueGame()
        if (currentPageIndex >= tutorialPages.Length - 1)
        {
            nextButtonText.text = "Continue";
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(ContinueGame);
            
            //Debug.Log("Continue button set - this should only happen on the last page");
        }
        else // Otherwise, show next
        {
            nextButtonText.text = "Next";
            nextButton.onClick.RemoveAllListeners();
            nextButton.onClick.AddListener(GoToNextPage);
            
            //Debug.Log("Next button set - this should happen on all pages except the last");
        }
    }


    void PauseGame()
    {
        Time.timeScale = 0f; 
        Cursor.lockState = CursorLockMode.None; 
        Cursor.visible = true; 
    }

    void ContinueGame()
    {
        tutorialsUI.SetActive(false); 
        DeactivateAllTutorialPages(); 
        ResumeGame();
    }

    void ResumeGame()
    {
        Time.timeScale = 1f; 
        Cursor.lockState = CursorLockMode.Locked; 
        Cursor.visible = false; 
    }

    void DeactivateAllTutorialPages()
    {
        // Deactivate all tutorial pages except the button panel
        foreach (Transform child in tutorialsUI.transform)
        {
            if (child.name != "BtnPanel") // Checks if child is not BtnPanel
            {
                child.gameObject.SetActive(false);
            }
        }
        // make sure pages array is deactivated.
        foreach (var page in tutorialPages)
        {
            page.SetActive(false);
        }
    }


 
    public void ResetTutorial()
    {
        currentPageIndex = 0;
        ShowPage(currentPageIndex);
        tutorialsUI.SetActive(false);
        ResumeGame();
    }

    void CheckSpaceBarInput()
    {
        // If the space bar is pressed and tutorials UI is active, ignore the input
        if (Input.GetKeyDown(KeyCode.Space) && tutorialsUI.activeSelf)
        {
            // Here you can handle what happens if space is pressed while UI is active
            // For now, we're just ignoring it
            Debug.Log("Space bar pressed but ignored due to active tutorial UI");
            return;
        }
    }
}
