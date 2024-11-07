using UnityEngine;
using System;

public class EnemyDetection : MonoBehaviour
{
    public float detectionRange = 50f;
    public float engageRange = 10f;
    public Transform player;
    public event Action OnPlayerDetected;

    public bool IsPlayerInRange() => Vector3.Distance(transform.position, player.position) <= detectionRange;
    public bool IsPlayerInEngageRange() => Vector3.Distance(transform.position, player.position) <= engageRange;

    void Update()
    {
        if (IsPlayerInRange())
            OnPlayerDetected?.Invoke();
    }
}
