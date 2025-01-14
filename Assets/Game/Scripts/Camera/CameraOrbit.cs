using UnityEngine;

public class CameraOrbit : MonoBehaviour
{
    public Transform player;  // Reference to the player
    public float rotationSpeed = 3f;  // Speed of the rotation
    public Vector3 offset = new Vector3(0f, 3f, -6f);  // Camera offset
    public float minYAngle = -10f;  // Minimum vertical rotation
    public float maxYAngle = 60f;  // Maximum vertical rotation
    public float deadZone = 0.1f;  // Dead zone to ignore small joystick inputs

    private float currentX = 0f;  // Horizontal rotation
    private float currentY = 10f;  // Vertical rotation

    private void LateUpdate()
    {
        // Get right stick input
        float rightStickX = Input.GetAxis("Mouse X");  // Horizontal axis (4th)
        float rightStickY = Input.GetAxis("Mouse Y");  // Vertical axis (5th)

        // Apply dead zone to prevent small unwanted movements
        if (Mathf.Abs(rightStickX) < deadZone) rightStickX = 0f;
        if (Mathf.Abs(rightStickY) < deadZone) rightStickY = 0f;

        // Update rotation based on joystick input
        currentX += rightStickX * rotationSpeed;
        currentY -= rightStickY * rotationSpeed;  // Subtract for natural feel

        // Clamp vertical rotation to avoid flipping
        currentY = Mathf.Clamp(currentY, minYAngle, maxYAngle);

        // Calculate new rotation and position
        Quaternion rotation = Quaternion.Euler(currentY, currentX, 0f);
        Vector3 desiredPosition = player.position + rotation * offset;

        // Update camera position and make it look at the player
        transform.position = desiredPosition;
        transform.LookAt(player.position);
    }
}
