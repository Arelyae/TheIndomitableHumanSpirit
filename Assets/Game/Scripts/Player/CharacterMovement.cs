using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    public float maxMovementSpeed = 5f;       // Max movement speed
    public float accelerationTime = 1f;       // Time to regain speed after turning
    public float decelerationTime = 0.3f;     // Time to stop
    public float quickTurnSpeed = 15f;        // Speed for quick turnaround
    public Transform cameraTransform;         // Reference to the camera transform

    private Rigidbody rb;
    private float currentSpeed = 0f;          // Current speed
    private Vector3 lastInputDirection = Vector3.zero;  // Store last input direction

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;  // Prevent unwanted Rigidbody rotation
    }

    public void Move(Vector3 movementInput)
    {
        // Convert input to world-space relative to the camera's direction
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // Flatten camera vectors to ignore vertical component
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 adjustedMovementInput = cameraForward * movementInput.z + cameraRight * movementInput.x;

        if (adjustedMovementInput.magnitude >= 0.1f)
        {
            float angleDifference = Vector3.Angle(lastInputDirection, adjustedMovementInput);

            if (angleDifference > 150f)  // If close to 180 degrees
            {
                // Stop movement and quickly turn
                currentSpeed = 0f;
                QuickTurn(adjustedMovementInput);
            }
            else
            {
                // Smooth acceleration to max speed
                currentSpeed = Mathf.MoveTowards(currentSpeed, maxMovementSpeed, maxMovementSpeed / accelerationTime * Time.fixedDeltaTime);
                Vector3 moveDirection = adjustedMovementInput.normalized * currentSpeed * Time.fixedDeltaTime;
                rb.MovePosition(rb.position + moveDirection);
            }

            // Store new direction
            lastInputDirection = adjustedMovementInput;
        }
        else
        {
            // Decelerate when stopping
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, maxMovementSpeed / decelerationTime * Time.fixedDeltaTime);
        }
    }

    public void Rotate(Vector3 movementInput)
    {
        if (movementInput.magnitude >= 0.1f)
        {
            // Convert input to world-space relative to the camera
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            cameraForward.y = 0f;
            cameraRight.y = 0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 adjustedMovementInput = cameraForward * movementInput.z + cameraRight * movementInput.x;

            float targetAngle = Mathf.Atan2(adjustedMovementInput.x, adjustedMovementInput.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    private void QuickTurn(Vector3 adjustedMovementInput)
    {
        // Instantly face the new direction
        float targetAngle = Mathf.Atan2(adjustedMovementInput.x, adjustedMovementInput.z) * Mathf.Rad2Deg;
        Quaternion quickTurnRotation = Quaternion.Euler(0f, targetAngle, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, quickTurnRotation, quickTurnSpeed * Time.deltaTime);
    }
}
