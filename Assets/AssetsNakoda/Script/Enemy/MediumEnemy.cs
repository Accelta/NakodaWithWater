using UnityEngine;
public class MediumEnemy : EnemyBase
{
    protected override void AssignStats()
    {
        health = 100f;          // Medium enemies have moderate health
        attackPower = 20f;      // Medium attack power
    }

    protected override float GetWanderSpeed()
    {
        return 15f;  // Medium wandering speed
    }

    protected override float GetChaseSpeed()
    {
        return maxSpeed;  // Medium chase speed
    }

    protected override void Die()
    {
        base.Die();
        Debug.Log("Medium enemy destroyed!");
        // Additional behavior on death can be added here
    }
}
