using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Floater))]
public abstract class EnemyBase : MonoBehaviour
{
    public enum State { Wandering, Chasing, Engaging }
    public State currentState = State.Wandering;

    public Transform player;

    [System.Serializable]
    public class Cannon
    {
        public Transform mainBody;          // Horizontal rotation (left/right)
        public Transform barrel;            // Vertical rotation (up/down)
        public Transform firePoint;         // Firing point
        public float maxHorizontalRotation = 60f;  // Horizontal rotation limit
        public float minVerticalRotation = -10f;   // Minimum vertical angle (down)
        public float maxVerticalRotation = 30f;    // Maximum vertical angle (up)
    }

    public Cannon[] cannons;            // Array of cannons on the enemy
    public float detectionRange = 50f;
    public float fireRate = 1f;
    public float bulletForce = 500f;
    public GameObject bulletPrefab;
    public float cannonRotationSpeed = 50f;

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
        spawnPoint = transform.position;
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
                    currentState = State.Engaging;
                break;

            case State.Engaging:
                EngagePlayer();
                RotateCannonsTowardPlayer();
                TryFireAtPlayer();
                if (!IsInEngageRange())
                    currentState = State.Chasing;
                break;
        }
        MoveEnemy();
    }

    void WanderAroundSpawn()
    {
        if (Vector3.Distance(transform.position, wanderTarget) <= 1f)
            SetWanderTarget();
        else
            MoveTowardsTarget(wanderTarget, GetWanderSpeed());
    }

    void SetWanderTarget()
    {
        Vector2 randomCircle = Random.insideUnitCircle * wanderRadius;
        wanderTarget = new Vector3(spawnPoint.x + randomCircle.x, transform.position.y, spawnPoint.z + randomCircle.y);
    }

    void LookForPlayer()
    {
        if (Vector3.Distance(transform.position, player.position) <= detectionRange)
            currentState = State.Chasing;
    }

    protected void ChasePlayer()
    {
        MoveTowardsTarget(player.position, maxSpeed);
    }

    protected void EngagePlayer()
    {
        Rigidbody playerRb = player.GetComponent<Rigidbody>();
        if (playerRb != null)
            MoveTowardsTarget(player.position, playerRb.velocity.magnitude);
    }

    bool IsInEngageRange() => Vector3.Distance(transform.position, player.position) <= engageRange;

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

    // Rotate each cannon toward the player within horizontal and vertical limits
   protected void RotateCannonsTowardPlayer()
{
    foreach (var cannon in cannons)
    {
        Vector3 directionToPlayer = (player.position - cannon.mainBody.position).normalized;

        // Separate rotation for horizontal (main body) and vertical (barrel) axes
        Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

        // Horizontal rotation for main body (left/right)
        float targetYaw = targetRotation.eulerAngles.y;
        if (targetYaw > 180f) targetYaw -= 360f;  // Normalize angle to [-180, 180]

        float currentYaw = cannon.mainBody.localEulerAngles.y;
        if (currentYaw > 180f) currentYaw -= 360f; // Normalize to [-180, 180]

        float clampedYaw = Mathf.Clamp(currentYaw + Mathf.DeltaAngle(currentYaw, targetYaw) * Time.deltaTime * cannonRotationSpeed,
            -cannon.maxHorizontalRotation, cannon.maxHorizontalRotation);

        cannon.mainBody.localRotation = Quaternion.Euler(0f, clampedYaw, 0f);

        // Vertical rotation for barrel (up/down)
        Vector3 localDirectionToPlayer = cannon.mainBody.InverseTransformDirection(directionToPlayer); // Local to main body
        float targetPitch = Mathf.Atan2(localDirectionToPlayer.y, localDirectionToPlayer.z) * Mathf.Rad2Deg;
        
        float currentPitch = cannon.barrel.localEulerAngles.x;
        if (currentPitch > 180f) currentPitch -= 360f;

        float clampedPitch = Mathf.Clamp(currentPitch + Mathf.DeltaAngle(currentPitch, targetPitch) * Time.deltaTime * cannonRotationSpeed,
            cannon.minVerticalRotation, cannon.maxVerticalRotation);

        cannon.barrel.localRotation = Quaternion.Euler(clampedPitch, 0f, 0f);
    }
}


    protected void TryFireAtPlayer()
    {
        if (Time.time >= nextFireTime)
        {
            foreach (var cannon in cannons)
                FireBullet(cannon.firePoint);

            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    protected void FireBullet(Transform firePoint)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.SetActive(true);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);

        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
            bulletScript.SetDamage(attackPower);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0) Die();
    }

    protected virtual void Die() => Destroy(gameObject);

    void AdjustDrag() => rb.drag = floater.underwater ? dragUnder : dragOver;

    protected abstract void AssignStats();

    protected abstract float GetWanderSpeed();

    protected abstract float GetChaseSpeed();
    //iuy
}
