using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;         // Movement speed
    [SerializeField] private float jumpForce = 10f;          // Force applied for jump
    [SerializeField] private Transform groundCheck;        // Position for ground check
    [SerializeField] private float groundCheckRadius = 0.2f; // Radius for ground check
    [SerializeField] private LayerMask groundLayer;          // Ground layer mask
    [SerializeField] private float gravityScale = 1.0f;
    [SerializeField] private float fallGravityMultiplier = 2.2f;
    [SerializeField] private float maxFallSpeed = 20f;

    private Rigidbody2D rb;
    private PlayerInput playerInput;
    private bool jumpRequested = false;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }

    private void Start()
    {
        rb.gravityScale = gravityScale;
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

        // if the player if moving downwards
        if (rb.linearVelocity.y < 0)
        {
            // Higher gravity if falling
            rb.gravityScale = gravityScale * fallGravityMultiplier;
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, Mathf.Max(rb.linearVelocity.y, -maxFallSpeed));
        }
    }

    // Check if the player is touching the ground using OverlapCircle
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
    }

    private void Run(float lerpAmount)
    {
        // TODO
    }
}