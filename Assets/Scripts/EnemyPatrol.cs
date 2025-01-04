using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class EnemyPatrol : MonoBehaviour
{
    public Transform[] waypoints; //for patrolling 
    public bool randomizeWaypoints; // Toggle for randomiser
    private List<int> waypointIndices; // Used for randomising the waypoints
    private int waypointIndex = 0;
    private NavMeshAgent agent;

    public float waitTimeAtWaypoint = 2f; 
    private float waitTimer;
    private Animator animator; 

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.autoBraking = false; // Prevents the agent from slowing down when appraoching a waypoint
        animator = GetComponent<Animator>(); 
        waypointIndices = new List<int>();

        InitializeWaypointIndices();
        MoveToNextWaypoint();
    }

    void InitializeWaypointIndices()
    {
        if (randomizeWaypoints)
        {
            RandomizeWaypoints();
        }
        else
        {
            for (int i = 0; i < waypoints.Length; i++)
            {
                waypointIndices.Add(i); // Fill in order
            }
        }
    }

    void MoveToNextWaypoint()
    {
        // Check if there are waypoints to follow.
        if (waypoints.Length == 0 || waypointIndices.Count == 0)
            return;

        // If at last waypoint, loop back to the first waypoint.
        if (waypointIndex >= waypointIndices.Count)
        {
            waypointIndex = 0; // Reset first waypoint index.
        }

        // Move NPC to waypoint.
        agent.SetDestination(waypoints[waypointIndices[waypointIndex]].position);
        animator.SetBool("IsWalking", true);

        // Increment waypoint index for next cycle.
        waypointIndex = (waypointIndex + 1) % waypoints.Length;

        //Debug.Log("Moving to next waypoint: " + waypointIndices[waypointIndex]);
    }





    void RandomizeWaypoints()
    {
        waypointIndices.Clear();
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypointIndices.Add(i);
        }
        ShuffleWaypointIndices();
    }

    void ShuffleWaypointIndices()
    {
        for (int i = 0; i < waypointIndices.Count; i++)
        {
            int temp = waypointIndices[i];
            int randomIndex = Random.Range(i, waypointIndices.Count);
            waypointIndices[i] = waypointIndices[randomIndex];
            waypointIndices[randomIndex] = temp;
        }
        waypointIndex = 0; // Reset waypoint index
    }

    void Update()
    {
        // Check if the NPC reached waypoint
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (animator.GetBool("IsWalking"))
            {
                animator.SetBool("IsWalking", false);
            }

            waitTimer += Time.deltaTime;

            // Check if the waiting time at the waypoint has been reached.
            if (waitTimer >= waitTimeAtWaypoint)
            {
                MoveToNextWaypoint();
                waitTimer = 0f;
            }
        }
        else
        {
            
            if (waitTimer != 0f)
            {
                waitTimer = 0f;
            }

            // Start walking animation
            if (!animator.GetBool("IsWalking") && agent.remainingDistance > agent.stoppingDistance)
            {
                animator.SetBool("IsWalking", true);
            }
        }

        // Debugging to check if the waypoint index is resetting correctly.
       // Debug.Log("Current Waypoint Index: " + waypointIndex);
       // Debug.Log("Current Waypoint Position: " + waypoints[waypointIndices[waypointIndex]].position);
        //Debug.Log("NPC Position: " + transform.position);
       // Debug.Log("Remaining Distance: " + agent.remainingDistance);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();

            // If its not found try get from parent GameObject
            if (playerHealth == null)
            {
                playerHealth = other.GetComponentInParent<PlayerHealth>();
            }

            if (playerHealth != null)
            {
                // Deal damage to player (Kills player)
                playerHealth.TakeDamage(playerHealth.currentHealth);
            }
            else
            {
                Debug.LogError("PlayerHealth component not found on Player!");
            }
        }
    }

}
