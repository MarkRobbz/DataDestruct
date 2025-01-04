using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuickDropDoorTrigger : MonoBehaviour
{
    public Animator doorAnimator;
    public int gemCost = 5; // Cost in gems to open the door
    private bool hasBeenPurchased = false; // Check if door has been purchased
    private bool isDoorOpen = false;
    private bool allowPrompt = true; //  control the prompt (Press E to buy message)
    private bool showNotEnoughGemsMessage = false; // control not enough gems message

    public GameObject interactionUI;
    private TextMeshProUGUI interactionTextUI;

    public TextMeshPro priceTextTMP;
    public RawImage priceImage;
    
    public AudioClip doorSwooshSound;

    private AudioSource doorAudioSource;

    private void Awake()
    {
        interactionTextUI = interactionUI.GetComponent<TextMeshProUGUI>();
        UpdatePriceUI();
        doorAudioSource = GetComponent<AudioSource>();
        
        if (doorAudioSource == null)
        {
            Debug.LogError("No AudioSource found on the GameObject.");
        }
    }


    private void UpdateInteractionText(string message)
    {
        interactionTextUI.text = message;
    }

    private void UpdatePriceUI()
    {
        priceTextTMP.text = gemCost.ToString();
    }

    private void HidePriceDisplay()
    {
        priceTextTMP.gameObject.SetActive(false);
        priceImage.gameObject.SetActive(false);
    }

    private IEnumerator ShowNotEnoughGemsMessage()
    {
        showNotEnoughGemsMessage = true;
        UpdateInteractionText("Not enough gems!");
        yield return new WaitForSeconds(2); // Display the message for 2 seconds
        showNotEnoughGemsMessage = false;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!hasBeenPurchased && !showNotEnoughGemsMessage)
            {
                if (allowPrompt)
                {
                    // Prompt the player to purchase the door
                    UpdateInteractionText($"Press E to pay {gemCost} gems to open door");
                    interactionUI.SetActive(true);
                }

                
                if (Input.GetKey(KeyCode.E) && allowPrompt)
                {
                    
                    allowPrompt = false; //stops need for button spaming to open dooor
                    TryOpenDoor();
                    StartCoroutine(EnablePromptAfterDelay());
                }
            }
            else if (hasBeenPurchased && !isDoorOpen)
            {
                // Automatically open the door when the player stays in the trigger
                OpenDoor();
                

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            interactionUI.SetActive(false);
            UpdateInteractionText(""); // Clear message when player exits the trigger box
            if (isDoorOpen)
            {
                // Automatically close door when the player exits the trigger
                CloseDoor();
            }
        }
    }

    public void TryOpenDoor()
    {
        if (!hasBeenPurchased)
        {
            if (GameManager.instance.gemCount >= gemCost)
            {
                GameManager.instance.gemCount -= gemCost; // Deduct cost
                GameManager.instance.UpdateGemCountUI(); // Update UI to reflect new gem count
                hasBeenPurchased = true; // door bought
                HidePriceDisplay();
                OpenDoor();
                allowPrompt = true; // Ensure prompt is enabled after successful purchase
            }
            else
            {
                Debug.Log("Not enough gems to open the door.");
                if (!showNotEnoughGemsMessage)
                {
                    StartCoroutine(ShowNotEnoughGemsMessage()); // Update the interaction message
                }
            }
        }
        else if (!isDoorOpen) // Ensure that the door opens if it's been purchased but is closed
        {
            OpenDoor();
        }
    }


    private IEnumerator EnablePromptAfterDelay()
    {
        yield return new WaitForSeconds(0.5f); // Wait for half a second
        allowPrompt = true;
    }
    private void OpenDoor()
    {
        doorAnimator.SetBool("OpenDoor", true);
        isDoorOpen = true;
        interactionUI.SetActive(false);
        doorAudioSource.clip = doorSwooshSound; 
        doorAudioSource.Play(); 
    }



    private void CloseDoor()
    {
        doorAnimator.SetBool("OpenDoor", false);
        isDoorOpen = false;
        doorAudioSource.clip = doorSwooshSound; 
        doorAudioSource.Play(); 
    }
}
