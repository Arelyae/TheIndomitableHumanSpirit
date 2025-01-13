using UnityEngine;

public class MovementHandler : MonoBehaviour
{
    public float maxMovementSpeed = 5f;       // Max movement speed
    public float accelerationTime = 1f;       // Time to regain speed after turning
    public float decelerationTime = 0.3f;     // Time to stop
    public float quickTurnSpeed = 15f;        // Speed for quick turnaround

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
        if (movementInput.magnitude >= 0.1f)
        {
            float angleDifference = Vector3.Angle(lastInputDirection, movementInput);

            if (angleDifference > 150f)  // If close to 180 degrees
            {
                // Stop movement and quickly turn
                currentSpeed = 0f;
                QuickTurn(movementInput);
            }
            else
            {
                // Smooth acceleration to max speed
                currentSpeed = Mathf.MoveTowards(currentSpeed, maxMovementSpeed, maxMovementSpeed / accelerationTime * Time.fixedDeltaTime);
                Vector3 moveDirection = transform.forward * currentSpeed * Time.fixedDeltaTime;
                rb.MovePosition(rb.position + moveDirection);
            }

            // Store new direction
            lastInputDirection = movementInput;
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
            float targetAngle = Mathf.Atan2(movementInput.x, movementInput.z) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
        }
    }

    private void QuickTurn(Vector3 movementInput)
    {
        // Instantly face the new direction
        float targetAngle = Mathf.Atan2(movementInput.x, movementInput.z) * Mathf.Rad2Deg;
        Quaternion quickTurnRotation = Quaternion.Euler(0f, targetAngle, 0f);
        transform.rotation = Quaternion.Slerp(transform.rotation, quickTurnRotation, quickTurnSpeed * Time.deltaTime);
    }
}
