using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Movement and jump parameters
    [SerializeField] private float moveSpeed = 5f;         // Movement speed
    [SerializeField] private float jumpForce = 10f;          // Jump force
    [SerializeField] private Transform groundCheck;        // Ground check position
    [SerializeField] private float groundCheckRadius = 0.2f; // Ground check radius
    [SerializeField] private LayerMask groundLayer;          // Ground layer mask
    [SerializeField] private float gravityScale = 1.0f;
    [SerializeField] private float fallGravityMultiplier = 2.2f;
    [SerializeField] private float maxFallSpeed = 20f;
    [SerializeField] private float jumpHangTimeThreshold = 1.0f;
    [SerializeField] private float jumpHangGravityMult = 0.5f;

    // Timer durations
    [SerializeField] private float coyoteTimeDuration = 0.1f;    // Duration after leaving ground where jump is still allowed
    [SerializeField] private float jumpBufferDuration = 0.1f;      // Duration to buffer jump input before landing

    // Private variables for physics and timers
    private Rigidbody2D rb;
    private PlayerInput playerInput; // Assuming a custom component for horizontal input
    private float coyoteTimer;       // Timer for coyote time
    private float jumpBufferTimer;   // Timer for jump buffer

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

    void FixedUpdate()
    {
        // Apply horizontal movement based on input
        rb.linearVelocity = new Vector2(playerInput.Horizontal * moveSpeed, rb.linearVelocity.y);

        // Check if jump is allowed: if jump was buffered and player is within coyote time
        if (jumpBufferTimer > 0f && coyoteTimer > 0f)
        {
            // Perform jump by setting vertical velocity
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            // Reset timers after jump is performed
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
        }

        // Modify gravity for jump hang effect when ascending slowly
        if (!IsGrounded() && Math.Abs(rb.linearVelocity.y) < jumpHangTimeThreshold)
        {
            rb.gravityScale = gravityScale * jumpHangGravityMult;
        }
        // Apply increased gravity while falling
        else if (rb.linearVelocity.y < 0)
        {
            rb.gravityScale = gravityScale * fallGravityMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
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
