using UnityEngine;
public class SmallEnemy : EnemyBase
{
    protected override void AssignStats()
    {
        health = 50f;          // Small enemies have lower health
        attackPower = 10f;      // Lower attack power
    }

    protected override float GetWanderSpeed()
    {
        return 10f;  // Fast wandering speed for small enemy
    }

    protected override float GetChaseSpeed()
    {
        return maxSpeed;  // Fast chase speed for small enemy
    }
    
    protected override void Die()
    {
        base.Die();
        Debug.Log("Small enemy destroyed!");
        // Additional behavior on death can be added here
    }
}
