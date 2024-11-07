using UnityEngine;
using System;

public class Health : MonoBehaviour
{
    public float maxHealth = 100f;
    private float currentHealth;

    public event Action OnDeath;

    void Start()
    {
        currentHealth = maxHealth;
    }

    public void InitializeHealth(float healthAmount)
    {
        maxHealth = healthAmount;
        currentHealth = maxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        currentHealth -= damageAmount;
        Debug.Log($"{gameObject.name} hit!");

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        OnDeath?.Invoke();
        Destroy(gameObject);
    }
}
