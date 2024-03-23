using System.Collections.Generic;
using UnityEngine;

public class FirstPersonMovement : MonoBehaviour
{
    public float speed = 5;

    [Header("Running")]
    public bool canRun = true;
    public bool IsRunning { get; private set; }
    public float runSpeed = 9;

    Rigidbody rigidbody;
    Camera mainCamera; // Reference to the main camera

    /// <summary> Functions to override movement speed. Will use the last added override. </summary>
    public List<System.Func<float>> speedOverrides = new List<System.Func<float>>();


    void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        mainCamera = Camera.main; // Get the main camera
    }

    void FixedUpdate()
    {
        IsRunning = canRun && (Input.GetAxis("Sprint") != 0);

        float targetMovingSpeed = IsRunning ? runSpeed : speed;
        if (speedOverrides.Count > 0)
        {
            targetMovingSpeed = speedOverrides[speedOverrides.Count - 1]();
        }

        // Get movement direction based on camera forward and right vectors
        Vector3 cameraForward = mainCamera.transform.forward;
        Vector3 cameraRight = mainCamera.transform.right;

        // Remove the vertical component of camera forward for movement on the ground plane
        cameraForward.y = 0f;

        // Get movement based on input and camera direction (horizontal only)
        float forwardAmount = Input.GetAxis("Vertical") * targetMovingSpeed; // No vertical movement from input
        float rightAmount = Input.GetAxis("Horizontal") * targetMovingSpeed;

        Vector3 targetHorizontalVelocity = cameraForward * forwardAmount + cameraRight * rightAmount;

        // Preserve current vertical velocity (for jumping)
        float currentVerticalVelocity = rigidbody.velocity.y;

        // Combine horizontal and vertical velocities
        Vector3 targetVelocity = new Vector3(targetHorizontalVelocity.x, currentVerticalVelocity, targetHorizontalVelocity.z);

        // Apply movement
        rigidbody.velocity = targetVelocity;
    }
}
