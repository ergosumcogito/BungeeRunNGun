using UnityEngine;
using UnityEngine.SceneManagement;

public class Death : MonoBehaviour
{
    // Method to handle player death
    void Die()
    {
        // Reload the current scene when the player dies
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Handle trigger-based deadly collisions (e.g., DeathZone, Spikes)
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collided object has a deadly tag
        if (other.CompareTag("Enemy") || other.CompareTag("DeathZone") || other.CompareTag("Spikes"))
        {
            Die();
        }
    }

    // Handle collision-based deadly contacts (e.g., enemy collision if not using triggers)
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collided object has the tag "Enemy"
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Die();
        }
    }
}