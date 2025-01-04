using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    PlayerHealth health;

    [Header("Movement")]
    public float moveSpeed;
    public float groundDrag;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitSlope;
    public float flatSurfaceThreshold; // How steep surface is, to be considered flat

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    private bool readyToJump;
    private int jumpsRemaining;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    // Vars for ground and fall detection
    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask isGround;
    bool grounded;
    [SerializeField] private float fallThresholdDistance;
    private bool wasGrounded;
    private float startOfFall;
    private bool wasFalling;

    // To track players direction
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDir;

    Rigidbody rigiBod;

    public PauseMenu pauseMenu;

    private Transform currentPlatform;
    private Vector3 platformLastPosition;

    public Animator animator; 

    private void Start()
    {
        rigiBod = GetComponent<Rigidbody>();
        rigiBod.freezeRotation = true;

        PlayerJumpReset();
        jumpsRemaining = 2;
        health = GetComponent<PlayerHealth>();

        startOfFall = transform.position.y; // Initialize startOfFall correctly

        if (health == null)
        {
            Debug.LogError("PlayerHealth component is not attached to the player GameObject.");
        }
        
       
        if (animator == null)
        {
            Debug.LogError("Animator component is not attached to the player GameObject.");
        }
    }

    public void Update()
    {
        // Prevents any further player input if the game is paused
        if (GameManager.instance.IsGamePaused)
            return;

        MyInput();
        SpeedController();

        if (grounded)
        {
            //rigiBod.drag = groundDrag;
        }
        else
        {
            rigiBod.drag = 0;
        }
        bool isMoving = horizontalInput != 0 || verticalInput != 0;
        animator.SetBool("IsRunning", isMoving); // Assuming 'IsRunning' is a boolean parameter in your Animator Controller

    }

    private void FixedUpdate()
    {
        // Prevents any further player movement/checks if the game is paused
        if (GameManager.instance.IsGamePaused)
            return;

        MovePlayer();
        CheckGrounded();
        CheckForPlayerFallDamage();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        if (Input.GetKeyDown(jumpKey) && jumpsRemaining > 0)
        {
            PlayerJump();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            // Toggle Pause Menu
            if (pauseMenu != null)
            {
                pauseMenu.TogglePauseMenu();
            }
            else
            {
                Debug.LogError("No active PauseMenu object found in the scene.");
            }
        }
    }

    private void MovePlayer()
    {
        moveDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // gives consistent force 
        moveDir.Normalize();

        // Apply a different force on slopes
        if (OnSlope() && !exitSlope)
        {
            Vector3 slopeDirection = Vector3.ProjectOnPlane(moveDir, slopeHit.normal);
            rigiBod.AddForce(slopeDirection * moveSpeed * 20f, ForceMode.Force);
        
            
            if (rigiBod.velocity.y > 0)
            {
                rigiBod.AddForce(Vector3.down * 50f, ForceMode.Force);
            }
        }
        else
        {
            // Regular movement on flat ground
            rigiBod.AddForce(moveDir * moveSpeed * 10f, ForceMode.Force);
        }
    
        
        if (!rigiBod.useGravity)
        {
            rigiBod.AddForce(Physics.gravity * rigiBod.mass, ForceMode.Force);
        }
    }


    private void SpeedController()
    {
        if (OnSlope() && !exitSlope)
        {
            if (rigiBod.velocity.magnitude > moveSpeed)
            {
                rigiBod.velocity = rigiBod.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rigiBod.velocity.x, 0f, rigiBod.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitVel = flatVel.normalized * moveSpeed;
                rigiBod.velocity = new Vector3(limitVel.x, rigiBod.velocity.y, limitVel.z);
            }
        }
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position + Vector3.down * playerHeight * 0.5f, Vector3.down, out slopeHit, playerHeight * 0.5f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle > flatSurfaceThreshold && angle < maxSlopeAngle;
        }
        return false;
    }

    private Vector3 GetSlopeMoveDir()
    {
        return Vector3.ProjectOnPlane(moveDir, slopeHit.normal).normalized;
    }

    private void PlayerJump()
    {
        exitSlope = true; // Set when jumping
        float adjustedJumpForce = jumpForce;
        if (OnSlope())
        {
            float slopeAngle = Vector3.Angle(Vector3.up, slopeHit.normal);
            adjustedJumpForce *= Mathf.Cos(slopeAngle * Mathf.Deg2Rad);
        }

        rigiBod.velocity = new Vector3(rigiBod.velocity.x, 0f, rigiBod.velocity.z);
        rigiBod.AddForce(transform.up * adjustedJumpForce, ForceMode.Impulse);
        jumpsRemaining--;

        Debug.Log("Jumping with adjusted force: " + adjustedJumpForce);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position + Vector3.down * playerHeight * 0.5f, transform.position + Vector3.down * playerHeight);
    }

    private void PlayerJumpReset()
    {
        readyToJump = true;
        exitSlope = false;
    }

    void CheckGrounded()
    {
        grounded = Physics.Raycast(transform.position + Vector3.down * playerHeight * 0.5f, Vector3.down, playerHeight * 0.5f, isGround);

        if (grounded)
        {
            
            if (jumpsRemaining == 0)
            {
                jumpsRemaining = 2;
                Debug.Log("JumpsRemaining reset (grounded): " + jumpsRemaining);
            }
            
            exitSlope = false; // Resets exitSlope flag when grounded
        }
    }

    bool IsFalling()
    {
        return !grounded && rigiBod.velocity.y < 0;
    }

    void TakeFallDamage()
    {
        float distanceFell = startOfFall - transform.position.y;
        if (distanceFell > fallThresholdDistance)
        {
            health.TakeDamage(1);
            Debug.Log("Take Damage" + " Player Fell " + (distanceFell));
        }
    }

    void CheckForPlayerFallDamage()
    {
        if (!wasFalling && IsFalling())
        {
            startOfFall = transform.position.y; // Set startOfFall when falling begins
        }
        else if (wasFalling && grounded)
        {
            TakeFallDamage();
        }

        wasGrounded = grounded;
        wasFalling = IsFalling();
    }

   

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("MovingPlatform"))
        {
            currentPlatform = other.transform;
            platformLastPosition = currentPlatform.position;
        }
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("MovingPlatform"))
        {
            if (currentPlatform != null)
            {
                Vector3 platformDeltaPosition = currentPlatform.position - platformLastPosition;
                transform.position += platformDeltaPosition;
                platformLastPosition = currentPlatform.position;
            }
            else
            {
                Debug.LogWarning("currentPlatform is null.");
            }
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform == currentPlatform)
        {
            currentPlatform = null;
        }
    }
}
