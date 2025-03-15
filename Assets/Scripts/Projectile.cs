using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float lifeTime = 5f; // Lifetime of the projectile

    void Start()
    {
        // Destroy the projectile after its lifetime expires
        Destroy(gameObject, lifeTime);
    }

    // Optionally, add collision detection logic here
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Destroy projectile on collision with any object
        Destroy(gameObject);
    }
}