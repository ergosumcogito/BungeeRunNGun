using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private GameObject projectilePrefab; // Projectile prefab
    [SerializeField] private Transform firePoint;         // Fire point transform
    [SerializeField] private float projectileSpeed = 10f;   // Speed of the projectile

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Shoot();
        }
    }

    void Shoot()
    {
        // Instantiate the projectile at the fire point position
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        // Get the Rigidbody2D component of the projectile
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            // Determine the direction based on player's facing direction
            // Using transform.localScale.x of the player, positive means right, negative means left
            float direction = transform.localScale.x > 0 ? 1f : -1f;
            rb.velocity = new Vector2(direction * projectileSpeed, 0f);
        }
    }
}