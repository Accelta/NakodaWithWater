using UnityEngine;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        currentHealth = maxHealth;
    }    
    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log("Player hit!");
        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        // Logic for death (destroy object, play animation, etc.)
        Debug.Log("Object died!");
        Destroy(gameObject);
    }
}
