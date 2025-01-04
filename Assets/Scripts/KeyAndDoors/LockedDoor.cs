using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    [SerializeField] private int keysRequired; 
    public GameObject door;
    public KeyPickup keyPickup; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (keyPickup != null && keyPickup.KeysPlayerHas >= keysRequired)
            {
                OpenDoor();
            }
            else
            {
                Debug.Log("Player does not have enough keys.");
            }
        }
    }

    private void OpenDoor()
    {
        if (door != null)
        {
            Animator doorAnimator = door.GetComponent<Animator>();
            if (doorAnimator != null)
            {
                doorAnimator.SetBool("Open", true);
            }
            else
            {
                Debug.LogError("No Animator found on door GameObject.");
            }
        }
        else
        {
            Debug.LogError("Door GameObject reference not set.");
        }
    }

    public int getKeysRequired()
    {
        return keysRequired;
    }
}
