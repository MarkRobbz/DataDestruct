using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NPCSystem : MonoBehaviour
{
    public string npcIdentifier;
    public string dialogue; // Full dialogue string
    private List<string> NPCDialoguePages; // List to hold dynamically created pages
    [SerializeField] private GameObject interactText;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;
    private TextMeshProUGUI dialogueTextComponent;
    private bool isPlayerNear = false;
    private int currentPageIndex = 0; // Current page index
    [SerializeField] private Transform playerTransform; //For following player

    private TextMeshProUGUI DialogueTextComponent
    {
        get
        {
            if (dialogueTextComponent == null && GameManager.instance.NPCDialogueUI != null)
            {
                dialogueTextComponent = GameManager.instance.NPCDialogueUI.GetComponentInChildren<TextMeshProUGUI>(true);
            }
            return dialogueTextComponent;
        }
    }

    void Start()
    {
        NPCDialoguePages = SegmentDialogue(dialogue, CalculateMaxCharactersPerPage());
        ResetDialogueState();
    }

    void Update()
    {
        if (GameManager.instance.IsChatGPTActive)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.E) && isPlayerNear)
        {
            if (GameManager.instance.GetCurrentNPC() != this)
            {
                currentPageIndex = 0; // Reset to first page for a new NPC
                GameManager.instance.SetCurrentNPC(this); // Update current NPC in GameManager
            }

            interactText.SetActive(false);
            GameManager.instance.ShowNPCDialogue();
            UpdateDialogueText(); // Update the dialogue text and button states
        }

        if (isPlayerNear)
        {
            FacePlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactText.SetActive(true);
            isPlayerNear = true;
            ResetDialogueState();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactText.SetActive(false);
            GameManager.instance.HideNPCDialogue();
            GameManager.instance.LockCursor();
            isPlayerNear = false;
            if (GameManager.instance.GetCurrentNPC() == this)
            {
                GameManager.instance.ClearCurrentNPC(); // Clear the current NPC since the player is no longer near
            }
        }
    }

    public void ShowNextDialoguePage()
    {
        if (currentPageIndex < NPCDialoguePages.Count - 1)
        {
            currentPageIndex++;
            UpdateDialogueText();
        }
    }

    public void ShowPreviousDialoguePage()
    {
        if (currentPageIndex > 0)
        {
            currentPageIndex--;
            UpdateDialogueText();
        }
    }

    private void UpdateDialogueText()
    {
        if (DialogueTextComponent != null && NPCDialoguePages.Count > currentPageIndex)
        {
            DialogueTextComponent.text = NPCDialoguePages[currentPageIndex];
        }

        nextButton.interactable = currentPageIndex < NPCDialoguePages.Count - 1;
        previousButton.interactable = currentPageIndex > 0;
    }

    private List<string> SegmentDialogue(string dialogue, int maxCharactersPerPage)
    {
        List<string> pages = new List<string>();
        string[] sentences = dialogue.Split(new char[] { '.', '!', '?' }, StringSplitOptions.RemoveEmptyEntries); // Splits into sentences
        string currentPageText = "";
        foreach (string sentence in sentences)
        {
            string trimmedSentence = sentence.Trim();
            if (!string.IsNullOrEmpty(trimmedSentence))
            {

                if (currentPageText.Length + trimmedSentence.Length + 2 <= maxCharactersPerPage) // +2 for punctuation and space
                {
                    currentPageText += (currentPageText.Length > 0 ? " " : "") + trimmedSentence + ".";
                }
                else
                {
                    // Add current page text to pages and start a new page with current sentence.
                    pages.Add(currentPageText);
                    currentPageText = trimmedSentence + ".";
                }
            }
        }
        // Add any remaining text as the last page.
        if (!string.IsNullOrWhiteSpace(currentPageText))
        {
            pages.Add(currentPageText);
        }
        return pages;
    }

    private int CalculateMaxCharactersPerPage()
    {
        RectTransform rectTransform = DialogueTextComponent.GetComponent<RectTransform>();
        float componentWidth = rectTransform.rect.width;
        float componentHeight = rectTransform.rect.height;

        // Estimate average char width (pixels) for font and size.
        float averageCharWidth = DialogueTextComponent.fontSize / 2f; // *Placeholder for average character width
        int charsPerLine = Mathf.FloorToInt(componentWidth / averageCharWidth);

        // Estimate line height as font size plus a rough estimate for line spacing.
        float lineHeight = DialogueTextComponent.fontSize * 1.2f;
        int linesPerPage = Mathf.FloorToInt(componentHeight / lineHeight); // Calculates the number of lines that can fit in the components height.

        // Calculate max chars per page.
        return charsPerLine * linesPerPage;
    }

    public void ResetDialogueState()
    {
        currentPageIndex = 0;
        UpdateDialogueText();
        nextButton.interactable = NPCDialoguePages.Count > 1;
        previousButton.interactable = false;

        nextButton.onClick.RemoveAllListeners();
        nextButton.onClick.AddListener(ShowNextDialoguePage);
        previousButton.onClick.RemoveAllListeners();
        previousButton.onClick.AddListener(ShowPreviousDialoguePage);
    }

    public void FacePlayer()
    {
        Vector3 directionToPlayer = playerTransform.position - transform.position;
        directionToPlayer.y = 0;
        Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
    }
}
