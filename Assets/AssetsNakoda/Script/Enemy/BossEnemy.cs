using UnityEngine;
public class BossEnemy : EnemyBase
{
    protected override void AssignStats()
    {
        health = 500f;          // Boss has much higher health
        attackPower = 50f;      // Higher attack power
    }

    protected override float GetWanderSpeed()
    {
        return 10f;  // Slower wandering speed for boss
    }

    protected override float GetChaseSpeed()
    {
        return maxSpeed;  // Slower chase speed for boss
    }

    protected override void Die()
    {
        base.Die();
        Debug.Log("Boss destroyed!");
        // Additional behavior on death can be added here
        // e.g., dropping special loot or triggering events
    }
}
