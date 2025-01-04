using UnityEngine;
using UnityEngine.UI;

public class ArrayAlleywayUI : MonoBehaviour
{
    [SerializeField]
    private GameObject arrayAlleyUI;
    public Image[] arraySlots;
    

    

    void OnTriggerEnter(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            arrayAlleyUI.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        
        if (other.CompareTag("Player"))
        {
            arrayAlleyUI.SetActive(false);
        }
    }

    public void AddItemToUI(int slotIndex, Sprite itemImage)
    {
        // Add the item to the UI
        if (slotIndex >= 0 && slotIndex < arraySlots.Length)
        {
            Transform itemBackgroundTransform = arraySlots[slotIndex].transform.Find("ItemBackground");
            if (itemBackgroundTransform != null)
            {
                Image itemImageComponent = itemBackgroundTransform.Find("ItemImage").GetComponent<Image>();
                if (itemImageComponent != null)
                {
                    itemImageComponent.sprite = itemImage;
                    itemImageComponent.enabled = true;
                }
                else
                {
                    Debug.LogWarning("ItemImage component not found in the selected arraySlot.");
                }
            }
            else
            {
                Debug.LogWarning("ItemBackground object not found in the selected arraySlot.");
            }
        }
        else
        {
            Debug.LogWarning("An invalid slotIndex was provided. Please check your TrashItem objects.");
        }
    }
}
