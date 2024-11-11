// using UnityEditor;
// using UnityEngine;

// [RequireComponent(typeof(Rigidbody))]
// [RequireComponent(typeof(Floater))]
// public class ShipController : MonoBehaviour
// {
//     public float maxSpeed = 20f;        // Maximum speed of the ship
//     public float acceleration = 5f;     // Speed increase rate
//     public float deceleration = 5f;     // Speed decrease rate
//     public float turnSpeed = 2f;        // Turning speed of the ship
//     public float rudderEffectiveness = 1f;  // Effectiveness of the rudder on the ship's turning
//     public float rudderTurnAngle = 30f; // Maximum rudder rotation angle

//     public float currentSpeed = 0f;    // Current speed of the ship
//     private float rudderInput = 0f;     // Input for the rudder (turning)
//     public float rudderSmoothSpeed = 5f;
//     private Rigidbody rb;
//     private Floater floater;
//     private bool underWater;
//      private float rotation;
//     public Transform rudder;  // Reference to the rudder object that will rotate

//     public float dragUnder, dragOver; // Drag values for underwater and above water

//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//         floater = GetComponent<Floater>();
//     }

//     void FixedUpdate()
//     {
//         AdjustDrag();
//         HandleSpeed();
//         HandleTurning();
//         MoveShip();
//         RotateRudder();
//         PreventUpsideDown();
//     }

//     // Adjust the drag of the ship depending on whether it's underwater or not
//     void AdjustDrag()
//     {
//         underWater = floater.underwater;
//         rb.drag = underWater ? dragUnder : dragOver;
//     }
//     void PreventUpsideDown()
//     {
//         rotation = Vector3.Angle(Vector3.up, transform.TransformDirection(Vector3.up));
//         if (rotation > 70f)
//         {
//             transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, transform.eulerAngles.y, 0f), 5f * Time.deltaTime);
//         }
//     }
//     // Handle the ship speed using W and S keys
//     void HandleSpeed()
//     {
//         if (Input.GetKey(KeyCode.W))
//         {
//             currentSpeed += acceleration * Time.deltaTime;
//         }
//         else if (Input.GetKey(KeyCode.S))
//         {
//             currentSpeed -= deceleration * Time.deltaTime;
//         }

//         // Clamp the speed between 0 and max speed
//         currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
//     }

//     // Handle the rudder and turning the ship
//     void HandleTurning()
//     {
//         // Only allow turning when the ship is moving (speed > 0)
//         if (currentSpeed > 0)
//         {
//             rudderInput = Input.GetAxis("Horizontal");  // A/D keys for rudder input
//         }
//         else
//         {
//             rudderInput = 0f;  // No turning if the ship is not moving
//         }
//     }

//     // Move the ship forward and apply rotation
//     void MoveShip()
//     {
//         // Move the ship forward based on the current speed
//         Vector3 forwardMovement = transform.forward * currentSpeed * Time.deltaTime;
//         rb.MovePosition(rb.position + forwardMovement);

//         // Apply rotation to the ship based on the rudder input
//         float turnAmount = rudderInput * turnSpeed * rudderEffectiveness * Time.deltaTime;
//         rb.MoveRotation(rb.rotation * Quaternion.Euler(0f, turnAmount, 0f));
//     }

//     // Rotate the rudder based on the turning input
//     void RotateRudder()
//     {
//         // Calculate the target rotation angle based on the input
//         float targetRudderRotation = -rudderInput * rudderTurnAngle;

//         if (rudder != null)
//         {
//             // Smoothly interpolate between the current and target rotation using Lerp
//             float smoothRotation = Mathf.LerpAngle(rudder.localRotation.eulerAngles.y, targetRudderRotation, Time.deltaTime * rudderSmoothSpeed);
            
//             // Apply the smooth rotation to the rudder
//             rudder.localRotation = Quaternion.Euler(0f, smoothRotation, 0f);
//         }
//     }
// }

using UnityEditor;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Floater))]
public class ShipController : MonoBehaviour
{
    public float maxSpeed = 20f;        // Maximum speed of the ship
    public float acceleration = 5f;     // Speed increase rate
    public float deceleration = 5f;     // Speed decrease rate
    public float turnSpeed = 2f;        // Turning speed of the ship
    public float rudderEffectiveness = 1f;  // Effectiveness of the rudder on the ship's turning
    public float rudderTurnAngle = 30f; // Maximum rudder rotation angle

    public float currentSpeed = 0f;    // Current speed of the ship
    private float rudderInput = 0f;     // Input for the rudder (turning)
    public float rudderSmoothSpeed = 5f;
    private Rigidbody rb;
    private Floater floater;
    private bool underWater;
    private float rotation;
    public Transform rudder;  // Reference to the rudder object that will rotate

    public float dragUnder, dragOver; // Drag values for underwater and above water

    public GameObject turnHelper; // A helper transform to apply turning forces

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        floater = GetComponent<Floater>();
    }

    void FixedUpdate()
    {
        AdjustDrag();
        HandleSpeed();
        HandleTurning();
        ApplyForces();
        RotateRudder();
        PreventUpsideDown();
    }

    // Adjust the drag of the ship depending on whether it's underwater or not
    void AdjustDrag()
    {
        underWater = floater.underwater;
        rb.drag = underWater ? dragUnder : dragOver;
    }

    void PreventUpsideDown()
    {
        rotation = Vector3.Angle(Vector3.up, transform.TransformDirection(Vector3.up));
        if (rotation > 70f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, transform.eulerAngles.y, 0f), 5f * Time.deltaTime);
        }
    }

    // Handle the ship speed using W and S keys
    void HandleSpeed()
    {
        if (Input.GetKey(KeyCode.W))
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            currentSpeed -= deceleration * Time.deltaTime;
        }

        // Clamp the speed between 0 and max speed
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);
    }

    // Handle the rudder and turning the ship
    void HandleTurning()
    {
        // Only allow turning when the ship is moving (speed > 0)
        if (currentSpeed > 0)
        {
            rudderInput = Input.GetAxis("Horizontal");  // A/D keys for rudder input
        }
        else
        {
            rudderInput = 0f;  // No turning if the ship is not moving
        }
    }

    // Apply forces to move and rotate the ship
    void ApplyForces()
    {
        if (turnHelper != null)
        {
            // Apply forward force based on the current speed
            Vector3 forwardForce = turnHelper.transform.forward * currentSpeed * rb.mass;
            rb.AddForce(forwardForce);

            // Apply torque to rotate the ship based on the rudder input
            float turnTorque = rudderInput * turnSpeed * rudderEffectiveness * rb.mass;
            rb.AddTorque(Vector3.up * turnTorque);
        }
    }

    // Rotate the rudder based on the turning input
    void RotateRudder()
    {
        // Calculate the target rotation angle based on the input
        float targetRudderRotation = -rudderInput * rudderTurnAngle;

        if (rudder != null)
        {
            // Smoothly interpolate between the current and target rotation using Lerp
            float smoothRotation = Mathf.LerpAngle(rudder.localRotation.eulerAngles.y, targetRudderRotation, Time.deltaTime * rudderSmoothSpeed);
            
            // Apply the smooth rotation to the rudder
            rudder.localRotation = Quaternion.Euler(0f, smoothRotation, 0f);
        }
    }
}