// using UnityEngine;
// [RequireComponent(typeof(Rigidbody))]
// [RequireComponent(typeof(Floater))]
// public abstract class EnemyBase : MonoBehaviour
// {
//     public enum State { Wandering, Chasing, Engaging }
//     public State currentState = State.Wandering;

//     public Transform player;
//     public Transform cannonMainBody;
//     public Transform cannonBarrel;

//     public float detectionRange = 50f;  // Distance at which enemy detects the player
//     public float fireRate = 1f;
//     public float bulletForce = 500f;
//     public GameObject bulletPrefab;
//     public Transform firePoint;
//     public float cannonRotationSpeed = 5f;
//     private Floater floater;
//     public float dragUnder = 2f;   // Drag for underwater
//     public float dragOver = 0.5f;  // Drag for above water

//     public float maxSpeed = 20f;        // Maximum speed of the enemy
//     public float acceleration = 5f;     // Speed increase rate
//     public float turnSpeed = 2f;        // Turning speed of the enemy
//     public float rudderEffectiveness = 1f;  // Effectiveness of the rudder on the enemy's turning
//     // public float rudderTurnAngle = 30f; // Maximum rudder rotation angle

//     private float currentSpeed = 0f;    // Current speed of the enemy
//     private float rudderInput = 0f;     // Input for the rudder (turning)
//     // public float rudderSmoothSpeed = 5f;
//     private Rigidbody rb;
//     private Vector3 spawnPoint;
//     private Vector3 wanderTarget;
//     private float nextFireTime = 0f;

//     protected float health;
//     protected float attackPower;
//     protected float wanderRadius = 20f;
//     public float engageRange = 10f;  // Range for engaging with the player

//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//           floater = GetComponent<Floater>();
//         spawnPoint = transform.position;  // Enemy's initial position as spawn point
//         SetWanderTarget();
//         AssignStats();
//     }

//     void FixedUpdate()
//     {
//          AdjustDrag();
//         switch (currentState)
//         {
//             case State.Wandering:
//                 WanderAroundSpawn();
//                 LookForPlayer();
//                 break;

//             case State.Chasing:
//                 ChasePlayer();
//                 RotateCannonTowardPlayer();
//                 if (IsInEngageRange())
//                 {
//                     currentState = State.Engaging;
//                 }
//                 break;

//             case State.Engaging:
//                 EngagePlayer();
//                 RotateCannonTowardPlayer();
//                 TryFireAtPlayer();
//                 if (!IsInEngageRange())
//                 {
//                     currentState = State.Chasing;
//                 }
//                 break;
//         }
//         MoveEnemy();
//     }

//     // Wandering around the spawn point
//     void WanderAroundSpawn()
//     {
//         if (Vector3.Distance(transform.position, wanderTarget) <= 1f)
//         {
//             SetWanderTarget();
//         }
//         else
//         {
//              MoveTowardsTarget(wanderTarget, GetWanderSpeed());
//         }
//     }

//     // Setting a new random target within the wander radius
//     void SetWanderTarget()
//     {
//         Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
//         wanderTarget = new Vector3(spawnPoint.x + randomCircle.x, transform.position.y, spawnPoint.z + randomCircle.y);
//     }

//     // Look for the player within detection range
//     void LookForPlayer()
//     {
//         float distanceToPlayer = Vector3.Distance(transform.position, player.position);
//         if (distanceToPlayer <= detectionRange)
//         {
//             currentState = State.Chasing;
//         }
//     }

//     // Chase the player
//     protected void ChasePlayer()
//     {
//          MoveTowardsTarget(player.position, maxSpeed);
//     }

//     // Engage the player by matching speed and firing
//     protected void EngagePlayer()
//     {
//         // // Match speed with the player
//         // currentSpeed = Mathf.MoveTowards(currentSpeed, player.GetComponent<Rigidbody>().velocity.magnitude, acceleration * Time.deltaTime);

//         // // Move towards the player
//         // MoveTowardsTarget(player.position);
//         Rigidbody playerRb = player.GetComponent<Rigidbody>();
//         if (playerRb != null)
//         {
//             MoveTowardsTarget(player.position, playerRb.velocity.magnitude);
//         }
//     }
//      bool IsInEngageRange()
//     {
//         return Vector3.Distance(transform.position, player.position) <= engageRange;
//     }

//     // Move towards a target position
//     void MoveTowardsTarget(Vector3 target, float targetspeed)
//     {
//         Vector3 direction = (target - transform.position).normalized;
//         float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
//         float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rudderInput, turnSpeed / rudderEffectiveness);
//         transform.rotation = Quaternion.Euler(0f, angle, 0f);

//         currentSpeed = Mathf.MoveTowards(currentSpeed, targetspeed, acceleration * Time.deltaTime);
//     }

//     // Move the enemy forward and apply rotation
//     void MoveEnemy()
//     {
//         Vector3 forwardMovement = transform.forward * currentSpeed * Time.deltaTime;
//         rb.MovePosition(rb.position + forwardMovement);
//     }

//     // Rotate the cannon to aim at the player
//     protected void RotateCannonTowardPlayer()
//     {
//         Vector3 directionToPlayer = (player.position - cannonMainBody.position).normalized;
//         Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

//         // Smoothly rotate the cannon toward the player
//         cannonMainBody.rotation = Quaternion.Slerp(cannonMainBody.rotation, lookRotation, cannonRotationSpeed * Time.deltaTime);

//         // Vertical barrel rotation (optional: limit up/down rotation)
//         Vector3 flatDirection = new Vector3(directionToPlayer.x, 0f, directionToPlayer.z);
//         float barrelAngle = Vector3.Angle(flatDirection, directionToPlayer);
//         cannonBarrel.localRotation = Quaternion.Euler(-barrelAngle, 0, 0);
//     }

//     // Fire at the player if within range
//     protected void TryFireAtPlayer()
//     {
//         if (Time.time >= nextFireTime)
//         {
//             FireBullet();
//             nextFireTime = Time.time + 1f / fireRate;
//         }
//     }

//     // Common firing behavior for all enemies
//     protected void FireBullet()
//     {
//         GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
//         bullet.SetActive(true);
//         Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
//         bulletRb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
//         // Set the bullet's damage to the enemy's attack power
//         Bullet bulletScript = bullet.GetComponent<Bullet>();
//         if (bulletScript != null)
//         {
//             bulletScript.SetDamage(attackPower);  // Assign damage based on enemy type
//         }
//     }

//     public void TakeDamage(float damage)
//     {
//         health -= damage;
//         Debug.Log("Enemy hit!");
//         if (health <= 0)
//         {
//             Die();
//         }
//     }

//     protected virtual void Die()
//     {
//         Destroy(gameObject);
//     }
//     void AdjustDrag()
//     {
//         rb.drag = floater.underwater ? dragUnder : dragOver;
//     }

//     // To be overridden by specific enemy types
//     protected abstract void AssignStats();

//     protected abstract float GetWanderSpeed();

//     protected abstract float GetChaseSpeed();
    
// }

using UnityEngine;
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Floater))]
public abstract class EnemyBase : MonoBehaviour
{
    public enum State { Wandering, Chasing, Engaging }
    public State currentState = State.Wandering;

    public Transform player;

    // Change cannonMainBody and cannonBarrel to arrays/lists for multiple cannons
    public Transform[] cannonMainBodies; // Main body of each cannon
    public Transform[] cannonBarrels;    // Barrel of each cannon
    public Transform[] firePoints;       // Fire points for each cannon

    public float detectionRange = 50f;  
    public float fireRate = 1f;
    public float bulletForce = 500f;
    public GameObject bulletPrefab;
    public float cannonRotationSpeed = 5f;

    private Floater floater;
    public float dragUnder = 2f;
    public float dragOver = 0.5f;

    public float maxSpeed = 20f;
    public float acceleration = 5f;
    public float turnSpeed = 2f;
    public float rudderEffectiveness = 1f;

    private float currentSpeed = 0f;
    private float rudderInput = 0f;
    private Rigidbody rb;
    private Vector3 spawnPoint;
    private Vector3 wanderTarget;
    private float nextFireTime = 0f;

    protected float health;
    protected float attackPower;
    protected float wanderRadius = 20f;
    public float engageRange = 10f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        floater = GetComponent<Floater>();
        spawnPoint = transform.position;  // Enemy's initial position as spawn point
        SetWanderTarget();
        AssignStats();
    }

    void FixedUpdate()
    {
        AdjustDrag();
        switch (currentState)
        {
            case State.Wandering:
                WanderAroundSpawn();
                LookForPlayer();
                break;

            case State.Chasing:
                ChasePlayer();
                RotateCannonsTowardPlayer();
                if (IsInEngageRange())
                {
                    currentState = State.Engaging;
                }
                break;

            case State.Engaging:
                EngagePlayer();
                RotateCannonsTowardPlayer();
                TryFireAtPlayer();
                if (!IsInEngageRange())
                {
                    currentState = State.Chasing;
                }
                break;
        }
        MoveEnemy();
    }

    void WanderAroundSpawn()
    {
        if (Vector3.Distance(transform.position, wanderTarget) <= 1f)
        {
            SetWanderTarget();
        }
        else
        {
            MoveTowardsTarget(wanderTarget, GetWanderSpeed());
        }
    }

    void SetWanderTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        wanderTarget = new Vector3(spawnPoint.x + randomCircle.x, transform.position.y, spawnPoint.z + randomCircle.y);
    }

    void LookForPlayer()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);
        if (distanceToPlayer <= detectionRange)
        {
            currentState = State.Chasing;
        }
    }

    protected void ChasePlayer()
    {
        MoveTowardsTarget(player.position, maxSpeed);
    }

    protected void EngagePlayer()
    {
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
        {
            MoveTowardsTarget(player.position, playerRb.velocity.magnitude);
        }
    }

    bool IsInEngageRange()
    {
        return Vector3.Distance(transform.position, player.position) <= engageRange;
    }

    void MoveTowardsTarget(Vector3 target, float targetSpeed)
    {
        Vector3 direction = (target - transform.position).normalized;
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref rudderInput, turnSpeed / rudderEffectiveness);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, acceleration * Time.deltaTime);
    }

    void MoveEnemy()
    {
        Vector3 forwardMovement = transform.forward * currentSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + forwardMovement);
    }

    // Rotate all cannons toward the player
    protected void RotateCannonsTowardPlayer()
    {
        for (int i = 0; i < cannonMainBodies.Length; i++)
        {
            Vector3 directionToPlayer = (player.position - cannonMainBodies[i].position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);

            // Smoothly rotate the cannon toward the player
            cannonMainBodies[i].rotation = Quaternion.Slerp(cannonMainBodies[i].rotation, lookRotation, cannonRotationSpeed * Time.deltaTime);

            // Optional vertical barrel rotation
            Vector3 flatDirection = new Vector3(directionToPlayer.x, 0f, directionToPlayer.z);
            float barrelAngle = Vector3.Angle(flatDirection, directionToPlayer);
            cannonBarrels[i].localRotation = Quaternion.Euler(-barrelAngle, 0, 0);
        }
    }

    // Fire from all cannons at the player
    protected void TryFireAtPlayer()
    {
        if (Time.time >= nextFireTime)
        {
            for (int i = 0; i < firePoints.Length; i++)
            {
                FireBullet(firePoints[i]);
            }
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    // Fire bullet from specific fire point
    protected void FireBullet(Transform firePoint)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.SetActive(true);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDamage(attackPower);
        }
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    void AdjustDrag()
    {
        rb.drag = floater.underwater ? dragUnder : dragOver;
    }

    protected abstract void AssignStats();

    protected abstract float GetWanderSpeed();

    protected abstract float GetChaseSpeed();
}
