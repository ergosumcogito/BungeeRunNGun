using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;         // Movement speed
    [SerializeField] private float jumpForce = 10f;          // Force applied for jump
    [SerializeField] private Transform groundCheck;        // Position for ground check
    [SerializeField] private float groundCheckRadius = 0.2f; // Radius for ground check
    [SerializeField] private LayerMask groundLayer;          // Ground layer mask

    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private bool jumpRequested = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    void Update()
    {
        // Check for jump input (space key) and ground state
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            jumpRequested = true;
        }
    }

    void FixedUpdate()
    {
        // Apply horizontal movement based on input
        rb.linearVelocity = new Vector2(playerInput.Horizontal * moveSpeed, rb.linearVelocity.y);

        // Apply jump force if jump is requested
        if (jumpRequested)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpRequested = false;
        }
    }

    // Check if the player is touching the ground using OverlapCircle
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }
}