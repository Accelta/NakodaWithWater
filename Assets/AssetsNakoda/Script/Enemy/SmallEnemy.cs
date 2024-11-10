using UnityEngine;

public class SmallEnemy : EnemyBase
{
    protected override void AssignStats()
    {
        healthComponent.maxHealth = 50f;  // Low health
        attackPower = 10f;                // Low attack power
        maxSpeed = 25f;                   // High speed
        fireRate = 1f;                    // High fire rate
    }

    protected override float GetWanderSpeed()
    {
        return maxSpeed * 0.4f;  // Speed while wandering
    }

    protected override float GetChaseSpeed()
    {
        return maxSpeed;  // Full speed while chasing
    }

    protected override void Die()
    {
        base.Die();
        Debug.Log("Small enemy destroyed!");
    }
}