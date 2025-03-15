using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Jump")]
    [SerializeField] private float jumpForce = 10f;          // Jump force
    [SerializeField] private Transform groundCheck;        // Ground check position
    [SerializeField] private float groundCheckRadius = 0.2f; // Ground check radius
    [SerializeField] private LayerMask groundLayer;          // Ground layer mask
    [SerializeField] private float gravityScale = 1.0f;
    [SerializeField] private float fallGravityMultiplier = 2.2f;
    [SerializeField] private float maxFallSpeed = 20f;
    [SerializeField] private float jumpHangTimeThreshold = 1.0f;
    [SerializeField] private float jumpHangGravityMult = 0.5f;
    
    [Space(5)]
    
    [SerializeField] private float coyoteTimeDuration = 0.2f;    // Duration after leaving ground where jump is still allowed
    [SerializeField] private float jumpBufferDuration = 0.2f;      // Duration to buffer jump input before landing
    
    [Space(10)]
    
    [Header("Movement")]
    [SerializeField] private float runMaxSpeed = 10f;         // Movement speed
    
    // Private variables for physics and timers
    private Rigidbody2D rb;
    private PlayerInput playerInput; // Custom component for horizontal input
    private float coyoteTimer;       // Timer for coyote time
    private float jumpBufferTimer;   // Timer for jump buffer
    private bool isFacingRight = true; // Facing direction

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Start()
    {
        rb.gravityScale = gravityScale;
    }

    void Update()
    {
        HandleInput();
        UpdateTimers();
        HandleFlip();
    }

    // Update input logic
    private void HandleInput()
    {
        // On jump button press, set the jump buffer timer
        if (Input.GetKeyDown(KeyCode.Space))
        {
            jumpBufferTimer = jumpBufferDuration;
        }
    }

    // Update timers each frame
    private void UpdateTimers()
    {
        // Decrease jump buffer timer
        jumpBufferTimer -= Time.deltaTime;

        // Update coyote timer: reset if grounded, else decrease
        if (IsGrounded())
        {
            coyoteTimer = coyoteTimeDuration;
        }
        else
        {
            coyoteTimer -= Time.deltaTime;
        }
    }
    
    // Flip player based on horizontal input
    private void HandleFlip()
    {
        if (playerInput.Horizontal > 0 && !isFacingRight)
        {
            Flip();
        }
        else if (playerInput.Horizontal < 0 && isFacingRight)
        {
            Flip();
        }
    }
    
    private void Flip()
    {
        // Multiply the player's x local scale by -1 to flip the sprite
        Vector3 localScale = transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
        isFacingRight = !isFacingRight;
    }

    void FixedUpdate()
    {
        Run();
        Jump();
    }
    
    void Run()
    {
        float horizontalInput = playerInput.Horizontal;

        if (IsGrounded() && Mathf.Abs(horizontalInput) < 0.01f)
        {
            rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocityX, 0, 1.0f), rb.linearVelocityY);
        }
        else
        {
            float targetSpeed = horizontalInput * runMaxSpeed;
            rb.linearVelocity = new Vector2(targetSpeed, rb.linearVelocityY);
        }
    }

    void Jump()
    {
        // Check if jump is allowed: if jump was buffered and player is within coyote time
        if (jumpBufferTimer > 0f && coyoteTimer > 0f)
        {
            // Perform jump by setting vertical velocity
            rb.linearVelocity = new Vector2(rb.linearVelocityX, jumpForce);
            // Reset timers after jump is performed
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }

        // Modify gravity for jump hang effect when ascending slowly
        if (!IsGrounded() && Math.Abs(rb.linearVelocityY) < jumpHangTimeThreshold)
        {
            rb.gravityScale = gravityScale * jumpHangGravityMult;
        }
        // Apply increased gravity while falling
        else if (rb.linearVelocityY < 0)
        {
            rb.gravityScale = gravityScale * fallGravityMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocityX, Mathf.Max(rb.linearVelocityY, -maxFallSpeed));
        }
        else
        {
            // Normal gravity otherwise
            rb.gravityScale = gravityScale;
        }
    }

    // Check if the player is touching the ground using OverlapCircle
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}
