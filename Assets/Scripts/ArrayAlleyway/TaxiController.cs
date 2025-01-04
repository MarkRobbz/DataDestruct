using UnityEngine;

public class TaxiController : MonoBehaviour
{
    public float speed = 12f;
    public float knockbackDistance = 5f; // for player
    public int damage = 1;

    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Move the taxi forward
        rb.MovePosition(transform.position + transform.forward * speed * Time.fixedDeltaTime);
    }

    void OnCollisionEnter(Collision collision)
    {
       // Debug.Log("Collision detected with: " + collision.gameObject.name);

        
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit by taxi.");

            // Calculate a backward offset off of the taxi's forward direction
            Vector3 backwardOffset = -transform.forward * knockbackDistance;

            // Move the player backward
            collision.transform.position += backwardOffset;

            
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                Debug.Log("Damage applied to player.");
            }
            else
            {
                Debug.LogError("PlayerHealth script not found on player object!");
            }
        }
    }


    void OnTriggerExit(Collider other)
    {
        // Check if the taxi has hitdespawn collider
        if (other.CompareTag("TaxisDespawnCollider"))
        {
            //Debug.Log("Taxi hit despawn collider. Destroying taxi.");
            Destroy(gameObject); // Destroy taxi
        }
    }

}
