using TMPro;
using UnityEngine;

public class KeyPickup : MonoBehaviour
{
    public int keysNeeded = 4;
    [SerializeField] private int keysPlayerHas = 0; // The current count of keys the player has
    public TextMeshProUGUI keyDisplayUI; // Reference to the UI text element that displays the key count

    
    public int KeysPlayerHas
    {
        get { return keysPlayerHas; }
        private set { keysPlayerHas = value; }
    }

    private void Start()
    {
        UpdateKeyDisplay(); // Initialize key display on start
    }

    private void OnTriggerEnter(Collider other)
    {
        Key key = other.GetComponent<Key>();
        if (key != null && KeysPlayerHas < keysNeeded)
        {
            AddKey();
            
            Destroy(key.gameObject); // Destroy the key GameObject after picking it up
            Debug.Log("Key Added");
        }
        else if (KeysPlayerHas >= keysNeeded)
        {
            Debug.Log($"You have collected all {KeysPlayerHas} keys and can't pick up anymore!!!");
        }
    }

    public void AddKey()
    {
        KeysPlayerHas++; // Increment the key count
        UpdateKeyDisplay(); // Update UI text to reflect new key count
        Debug.Log("Key Added");
    }

    private void UpdateKeyDisplay()
    {
        // Update UI Text to show the current count of keys
        if (keyDisplayUI != null)
        {
            keyDisplayUI.text = $"{KeysPlayerHas}/{keysNeeded}";
        }
    }
}
