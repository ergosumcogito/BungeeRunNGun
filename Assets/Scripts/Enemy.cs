using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 2f; // Movement speed of the enemy
    private int moveDirection = 1; // 1 for right, -1 for left
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Get the SpriteRenderer component attached to the enemy
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        // Move enemy horizontally
        transform.Translate(Vector2.right * speed * moveDirection * Time.deltaTime);
        // Flip sprite based on movement direction
        spriteRenderer.flipX = moveDirection < 0;
    }

    // Reverse direction when colliding with path triggers
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("EnemyPathTrigger"))
        {
            // Reverse the enemy movement direction
            moveDirection *= -1;
        }
        
        if (other.gameObject.CompareTag("PlayerBullet"))
        {
            Destroy(gameObject);
        }
    }
}