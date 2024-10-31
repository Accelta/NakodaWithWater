using UnityEngine;

public class CannonRotation : MonoBehaviour
{
    public Transform mainBody;  // The main body of the cannon (sphere for left/right)
    public Transform barrel;    // The barrel of the cannon (cube for up/down)

    public GameObject bulletPrefab;     // Assign the bullet prefab in the Inspector
    public Transform firePoint;         // A point on the barrel where bullets are fired from
    public float bulletForce = 500f;    // Force applied to the bullet when fired
    public float fireRate = 1f;         // Time between shots
    private float nextFireTime = 0f;    // Time until the next shot is allowed

    public float rotationSpeed = 50f;   // Speed of the rotation
    public float maxHorizontalRotation = 60f;  // Limit how much the cannon can turn left/right
    public float minVerticalRotation = -10f;   // Minimum tilt of the barrel (down)
    public float maxVerticalRotation = 30f;    // Maximum tilt of the barrel (up)
    public float playerDamage = 20f;   // Damage dealt to the player when hit

    private float currentHorizontalRotation;
    private float currentVerticalRotation;
    public bool isRotationActive = false;

    void Start()
    {
        currentHorizontalRotation = mainBody.localEulerAngles.y;
        currentVerticalRotation = barrel.localEulerAngles.x;
    }

    void Update()
    {
        if (isRotationActive)
        {
            HandleCannonRotation();

            // Handle firing the bullet
            if (Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
            {
                FireBullet();
                nextFireTime = Time.time + fireRate;  // Update the next fire time
            }
        }
    }

    void HandleCannonRotation()
    {
        // Horizontal rotation (main body left/right)
        float horizontalInput = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float targetHorizontalRotation = currentHorizontalRotation + horizontalInput;
        float deltaHorizontalRotation = Mathf.DeltaAngle(0, targetHorizontalRotation);
        currentHorizontalRotation = Mathf.Clamp(deltaHorizontalRotation, -maxHorizontalRotation, maxHorizontalRotation);
        Quaternion horizontalRotation = Quaternion.Euler(0, currentHorizontalRotation, 0);
        mainBody.localRotation = horizontalRotation;

        // Vertical rotation (barrel up/down)
        float verticalInput = -Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;
        currentVerticalRotation = Mathf.Clamp(currentVerticalRotation + verticalInput, minVerticalRotation, maxVerticalRotation);
        Quaternion verticalRotation = Quaternion.Euler(currentVerticalRotation, 0, 0);
        barrel.localRotation = verticalRotation;
    }

    void FireBullet()
    {
        // Create a bullet at the fire point's position and rotation
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        bullet.SetActive(true);  // Ensure the bullet is active
        Debug.Log("Bullet Fired");

        // Get the Rigidbody of the bullet and add force to it
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        if (bulletRb != null)
        {
            // Apply force in the direction the barrel is facing
            bulletRb.AddForce(firePoint.forward * bulletForce, ForceMode.Impulse);
            Debug.Log("Bullet Force Applied");
        }
        else
        {
            Debug.LogError("Rigidbody not found on the bullet!");
        }
        // Set the bullet's damage for the player
        Bullet bulletScript = bullet.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.SetDamage(playerDamage);
        }
    }
}
