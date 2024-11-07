using UnityEngine;

public class MediumEnemy : EnemyBase
{
    protected override void AssignStats()
    {
        healthComponent.maxHealth = 100f;  // Medium health
        attackPower = 20f;                 // Medium attack power
        maxSpeed = 20f;                    // Medium speed
        fireRate = 1f;                     // Medium fire rate
    }

    protected override float GetWanderSpeed()
    {
        return maxSpeed * 0.75f;  // Speed while wandering
    }

    protected override float GetChaseSpeed()
    {
        return maxSpeed;  // Full speed while chasing
    }

    protected override void Die()
    {
        base.Die();
        Debug.Log("Medium enemy destroyed!");
    }
}