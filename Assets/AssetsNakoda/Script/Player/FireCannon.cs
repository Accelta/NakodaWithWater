using UnityEngine;

public class FireCannon : MonoBehaviour
{
    public GameObject bulletPrefab;  // The bullet prefab to be instantiated
    public Transform barrelEnd;      // The point where the bullet will be fired from (the end of the barrel)
    public float bulletSpeed = 20f;  // The initial speed of the bullet
    public float fireRate = 1f;      // Time between shots

    private float nextFireTime = 0f;

    void Update()
    {

        HandleFireInput();
    }

    void HandleFireInput()
    {
        // Fire the cannon when the player presses the left mouse button (or another key) and respects fire rate
        if (Input.GetMouseButton(0) && Time.time > nextFireTime)
        {
            FireBullet();
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    void FireBullet()
    {
        // Instantiate the bullet at the end of the barrel with the barrel's rotation
        GameObject bullet = Instantiate(bulletPrefab, barrelEnd.position, barrelEnd.rotation);

        // Get the Rigidbody component and apply force to simulate firing
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

        // Apply a forward force (using the barrel's forward direction) to the bullet
        bulletRb.velocity = barrelEnd.forward * bulletSpeed;
    }
}
