using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public CinemachineVirtualCamera shipVirtualCamera;    // Reference to the ship's virtual camera
    public CinemachineVirtualCamera cannonVirtualCamera;  // Reference to the cannon's virtual camera
    public CannonRotation cannonRotation;  // Reference to the CannonRotation script
    public Transform playerBody;  // Reference to the player's body for rotating around

    public float rotationSpeed = 100f;  // Speed of camera rotation
    public float transitionSpeed = 2f;  // Speed of returning to the original position

    private bool isUsingCannon = false;
    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private bool isRotatingCamera = false;
    private bool isReturningToPosition = false;  // Flag for transitioning back

    void Start()
    {
        // Save the original camera position and rotation
        originalCameraPosition = shipVirtualCamera.transform.localPosition;
        originalCameraRotation = shipVirtualCamera.transform.localRotation;
    }

    void Update()
    {
        HandleCameraSwitch();
        HandleCameraRotation();
        HandleCameraTransitionBack();
    }

    // Switch between ship and cannon camera
    void HandleCameraSwitch()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            isUsingCannon = !isUsingCannon;

            // Toggle between ship camera and cannon camera
            shipVirtualCamera.gameObject.SetActive(!isUsingCannon);
            cannonVirtualCamera.gameObject.SetActive(isUsingCannon);

            // Enable cannon rotation only when using the cannon
            cannonRotation.isRotationActive = isUsingCannon;

            // Lock the cursor when using the cannon camera
            if (isUsingCannon)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    // Handle camera rotation around the player
    void HandleCameraRotation()
    {
        if (!isUsingCannon)
        {
            if (Input.GetMouseButtonDown(1)) // Right mouse button pressed
            {
                isRotatingCamera = true;
                isReturningToPosition = false;  // Stop returning to position while rotating
            }
            if (Input.GetMouseButtonUp(1)) // Right mouse button released
            {
                isRotatingCamera = false;
                isReturningToPosition = true;  // Start the smooth transition back
            }

            if (isRotatingCamera)
            {
                RotateAroundPlayer();  // Rotate around the player
            }
        }
    }

    // Rotate the camera around the player
    void RotateAroundPlayer()
    {
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;

        // Rotate around the player body on the Y-axis
        shipVirtualCamera.transform.RotateAround(playerBody.position, Vector3.up, mouseX);
    }

    // Handle smooth transition back to the original position
    void HandleCameraTransitionBack()
    {
        if (isReturningToPosition)
        {
            // Smoothly move the camera back to the original position
            shipVirtualCamera.transform.localPosition = Vector3.Lerp(
                shipVirtualCamera.transform.localPosition, 
                originalCameraPosition, 
                transitionSpeed * Time.deltaTime);

            // Smoothly rotate the camera back to the original rotation
            shipVirtualCamera.transform.localRotation = Quaternion.Slerp(
                shipVirtualCamera.transform.localRotation, 
                originalCameraRotation, 
                transitionSpeed * Time.deltaTime);

            // Stop transitioning if the camera is close to the original position and rotation
            if (Vector3.Distance(shipVirtualCamera.transform.localPosition, originalCameraPosition) < 0.01f &&
                Quaternion.Angle(shipVirtualCamera.transform.localRotation, originalCameraRotation) < 0.01f)
            {
                isReturningToPosition = false;  // Transition complete
            }
        }
    }
}
