using UnityEngine;

public class ShipBuoyancy : MonoBehaviour
{
    public Transform water;             // The water object with the WaveWireframe script
    public float buoyancyStrength = 5f; // Strength of the buoyancy force
    public float floatHeight = 1.5f;    // Desired height above the water
    public float damping = 0.2f;        // Dampens the motion to reduce bouncing
    public Rigidbody rb;                // Rigidbody of the ship
    
    private WaveWireFrame wave;         // Reference to the WaveWireframe script

    void Start()
    {
        // Get the WaveWireframe script from the water object
        if (water != null)
        {
            wave = water.GetComponent<WaveWireFrame>();
        }

        // Ensure the Rigidbody is attached
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();
        }
    }

    void FixedUpdate()
    {
        if (wave != null)
        {
            ApplyBuoyancy();
        }
    }

    void ApplyBuoyancy()
    {
        // Get the wave height at the ship's position
        float waveHeight = wave.GetWaveHeightAtPosition(transform.position);

        // Calculate the height difference between the ship and the wave
        float heightDifference = (waveHeight + floatHeight) - transform.position.y;

        // Calculate the buoyancy force and apply it
        if (heightDifference > 0)
        {
            float buoyancyForce = heightDifference * buoyancyStrength;
            rb.AddForce(Vector3.up * buoyancyForce, ForceMode.Acceleration);
        }

        // Apply damping to reduce bouncing
        rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y * (1 - damping), rb.velocity.z);
    }
}
