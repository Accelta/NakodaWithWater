// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;

// public class BoatController : MonoBehaviour
// {
//     private float horizontal;
//     private float vertical;
//     public Transform motorPosition;
//     public float speed;
//     public AnimationCurve accelerationCurve = new AnimationCurve(new Keyframe(0f, 0f), new Keyframe(1f, 1f));

//     public float turnSpeed;
//     public float tiltForce;

//     private float rotation;
//     private Rigidbody rb;
//     private bool underWater;

//     public GameObject turnHelper;
//     public Floater floater;

//     public float elapsedTime, elapsedTimeBack;

//     public float dragUnder, dragOver;

//     // Start is called before the first frame update
//     void Start()
//     {
//         rb = GetComponent<Rigidbody>();
//     }


//     private void FixedUpdate()
//     {
//         //prevent upside down
//         rotation = Vector3.Angle(Vector3.up, transform.TransformDirection(Vector3.up));
//         if (rotation > 70f)
//             transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, transform.eulerAngles.y, 0f), 5f * Time.deltaTime);

//         // Check if in water
//         if (floater.underwater)
//         {
//             underWater = true;
//             rb.drag = dragUnder;
//         }
//         else
//         {
//             underWater = false;
//             rb.drag = dragOver;
//         }


//         if (underWater && turnHelper != null)
//         {
//             rb.AddTorque(transform.up * horizontal * 100f * turnSpeed * Time.deltaTime); //turning

//             if (vertical > 0.1f)
//             {
//                 float evaluatedCurve = accelerationCurve.Evaluate(elapsedTime);
//                 rb.AddForce(turnHelper.transform.forward * speed * evaluatedCurve * 0.05f * vertical * Time.deltaTime * 300f, ForceMode.Force);  //moving
//                 rb.AddTorque(transform.right * tiltForce * -vertical * Time.deltaTime, ForceMode.Force); //optional tilt 
//             }
//             if (vertical < -0.1f)
//             {
//                 float evaluatedCurve = accelerationCurve.Evaluate(elapsedTimeBack);
//                 rb.AddForce(turnHelper.transform.forward * speed * evaluatedCurve * 0.02f * vertical * Time.deltaTime * 300f, ForceMode.Force);  //moving  
//             }

//         }
//     }


//     // Update is called once per frame
//     void Update()
//     {
//         horizontal = Input.GetAxisRaw("Horizontal");
//         vertical = Input.GetAxisRaw("Vertical");


//         if (vertical <= 0f && elapsedTime > 0f)
//         {
//             elapsedTime -= Time.deltaTime;

//         }
//         if (vertical >= 0f && elapsedTimeBack > 0f)
//         {

//             elapsedTimeBack -= Time.deltaTime;
//         }
//         if (vertical >= 0.1f && elapsedTime < 1f)
//             elapsedTime += Time.deltaTime;
//         if (vertical <= -0.1f && elapsedTimeBack < 1f)
//             elapsedTimeBack += Time.deltaTime;
//     }
// }

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    private float horizontal;
    private float vertical;

    public Transform motorPosition; // Reference to the boat's motor or thrust position
    public float maxSpeed = 20f;    // Maximum speed of the boat
    public float acceleration = 5f; // Speed increase rate
    public float deceleration = 5f; // Speed decrease rate

    public float turnSpeed = 2f;    // Turning speed of the boat
    public float tiltForce = 10f;   // Tilt force applied when turning
    public float rudderEffectiveness = 1f;  // Effectiveness of the rudder on the boat's turning

    public float currentSpeed = 0f; // Current speed of the boat
    private float rotation;
    private Rigidbody rb;
    private bool underWater;

    public GameObject turnHelper; // A helper transform to apply turning forces
    public Floater floater;       // Reference to the floater component

    public float dragUnder, dragOver; // Drag values for underwater and above water

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // Prevent the boat from flipping upside down
        PreventUpsideDown();

        // Check if the boat is in water and adjust drag
        AdjustDrag();

        if (underWater && turnHelper != null)
        {
            HandleTurning();
            HandleMovement();
        }
    }

    // Prevent the boat from flipping upside down
    void PreventUpsideDown()
    {
        rotation = Vector3.Angle(Vector3.up, transform.TransformDirection(Vector3.up));
        if (rotation > 70f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.Euler(0f, transform.eulerAngles.y, 0f), 5f * Time.deltaTime);
        }
    }

    // Adjust the drag of the boat depending on whether it's underwater or not
    void AdjustDrag()
    {
        underWater = floater.underwater;
        rb.drag = underWater ? dragUnder : dragOver;
    }

    // Handle the boat's turning using A/D keys
    void HandleTurning()
    {
        horizontal = Input.GetAxisRaw("Horizontal");

        // Apply torque to turn the boat based on horizontal input
        rb.AddTorque(transform.up * horizontal * turnSpeed * rudderEffectiveness * Time.deltaTime);

        // Optional tilt while turning
        rb.AddTorque(transform.right * -tiltForce * horizontal * Time.deltaTime, ForceMode.Force);
    }

    // Handle the boat's speed using W/S keys
    void HandleMovement()
    {
        // Increase speed when W is pressed, decrease when S is pressed
        if (Input.GetKey(KeyCode.W))
        {
            currentSpeed += acceleration * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            currentSpeed -= deceleration * Time.deltaTime;
        }

        // Clamp the speed to be between 0 and maxSpeed
        currentSpeed = Mathf.Clamp(currentSpeed, 0f, maxSpeed);

        // Apply the forward movement based on the current speed
        rb.AddForce(turnHelper.transform.forward * currentSpeed * Time.deltaTime * 300f, ForceMode.Force);
    }

    // Update is called once per frame
    void Update()
    {
        // No need to update elapsedTime here as the movement is now controlled by W and S keys
    }
}
