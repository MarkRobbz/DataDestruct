using UnityEngine;
using TMPro;
using System.Collections; // Required for IEnumerator

public class ItemPickup : MonoBehaviour
{
    public GameObject pickupText;
    public ArrayAlleywayUI hudArray;
    public TextMeshProUGUI itemCountText; // Reference to your TMP text
    public float displayDuration = 3.0f; // Duration for which the text is displayed

    private bool canPickUp = false;
    private GameObject currentTrashObject = null;
    private int itemCount = 0; // Counter for the collected items
    private const int TotalItemCount = 6; // Total items to collect for the task

    private void Start()
    {
        pickupText.SetActive(false);
        itemCountText.gameObject.SetActive(false); // Ensure the text is initially hidden
    }

    private void Update()
    {
        if (canPickUp && Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E key pressed");
            if (currentTrashObject != null)
            {
                PickTrashUp(currentTrashObject);
                pickupText.SetActive(false);
                canPickUp = false;
                currentTrashObject = null;
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Trash"))
        {
            pickupText.SetActive(true);
            canPickUp = true;
            currentTrashObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Trash"))
        {
            pickupText.SetActive(false);
            canPickUp = false;
            currentTrashObject = null;
        }
    }

    private void PickTrashUp(GameObject trashObject)
    {
        TrashItem trashItem = trashObject.GetComponent<TrashItem>();

        if (trashItem != null)
        {
            int trashIndex = trashItem.trashIndex;
            hudArray.AddItemToUI(trashIndex, trashItem.image);
            Destroy(trashObject);

            itemCount++;
            StartCoroutine(ShowItemCountText());

            if (itemCount >= TotalItemCount)
            {
                GameManager.instance.CompleteTaskInSection("ArrayAlleyway");
            }
        }
        else
        {
            Debug.Log("TrashItem component not found on: " + trashObject.name);
        }
    }

    private IEnumerator ShowItemCountText()
    {
        UpdateItemCountText();
        itemCountText.gameObject.SetActive(true);
        yield return new WaitForSeconds(displayDuration);
        itemCountText.gameObject.SetActive(false);
    }

    private void UpdateItemCountText() //After each trash picup it promps text on screen
    {
        if (itemCountText != null)
        {
            itemCountText.text = $"{itemCount}/{TotalItemCount} items collected";
            itemCountText.color = Color.green;
        }
    }
}
