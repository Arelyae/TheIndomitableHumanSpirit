using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player;  // Player's Transform (drag and drop in the inspector)
    public Vector3 offset = new Vector3(0f, 5f, -10f);  // Camera offset from player
    public float smoothSpeed = 0.125f;  // Smooth damping speed

    private void LateUpdate()
    {
        // Keep camera position updated to player's position + offset
        Vector3 desiredPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        // Lock the rotation (camera stays independent of player's rotation)
        transform.rotation = Quaternion.Euler(30f, 0f, 0f);  // Change angles to your liking
    }
}
