using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;  // Movement speed
    private Rigidbody2D rb;
    private PlayerInput playerInput;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        playerInput = GetComponent<PlayerInput>();
    }
    
    void FixedUpdate()
    {
        // Apply movement based on input
        rb.linearVelocity = new Vector2(playerInput.Horizontal * moveSpeed, rb.linearVelocity.y);
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
