using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float damage;
    public float lifetime = 5f; // Lifetime of the bullet in seconds

    void Start()
    {
        // Schedule the bullet to be destroyed after its lifetime
        Destroy(gameObject, lifetime);
    }

    // Method to set damage from the enemy or player
    public void SetDamage(float damageAmount)
    {
        damage = damageAmount;
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the collided object is an enemy
        if (other.TryGetComponent<EnemyBase>(out EnemyBase enemy))
        {
            // Apply damage to the enemy
            enemy.TakeDamage(damage);
        }

        // Check if the collided object is the player
        if (other.TryGetComponent<Health>(out Health playerHealth))
        {
            // Apply damage to the player
            playerHealth.TakeDamage(damage);
        }

        // Destroy the bullet after hitting something
        Destroy(gameObject);
    }
}
