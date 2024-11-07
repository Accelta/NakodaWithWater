using UnityEngine;

public class BossEnemy : EnemyBase
{
    protected override void AssignStats()
    {
        healthComponent.maxHealth = 500f;  // High health
        attackPower = 50f;                 // High attack power
        maxSpeed = 10f;                    // Low speed
        fireRate = 0.5f;                   // Low fire rate
    }

    protected override float GetWanderSpeed()
    {
        return maxSpeed * 0.5f;  // Speed while wandering
    }

    protected override float GetChaseSpeed()
    {
        return maxSpeed;  // Full speed while chasing
    }

    protected override void Die()
    {
        base.Die();
        Debug.Log("Boss destroyed!");
    }
}