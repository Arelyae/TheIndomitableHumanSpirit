using UnityEngine;

public class ZTargeting : MonoBehaviour
{
    public Transform player;  // Reference to the player
    public Transform targetPoint;  // The point that will be positioned at the center of the line
    public float smoothSpeed = 5f;  // Speed for smooth movement
    public float maxDistance = 20f;  // Maximum allowable distance for linking

    private Transform objectB;  // The current target (nearest enemy)
    private Vector3 desiredPosition;  // The desired position for the Target Point
    private bool isTargeting = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TryStartZTargeting();
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            StopZTargeting();
        }

        // Smoothly move the target point toward the desired position
        if (targetPoint != null)
        {
            targetPoint.position = Vector3.Lerp(targetPoint.position, desiredPosition, Time.deltaTime * smoothSpeed);
        }

        // Update the desired target point position while targeting
        if (isTargeting && player != null && objectB != null)
        {
            float currentDistance = Vector3.Distance(player.position, objectB.position);

            // Break targeting if the distance exceeds the maximum threshold
            if (currentDistance > maxDistance)
            {
                Debug.Log("Target too far: Breaking Z-targeting");
                StopZTargeting();
                return;
            }

            desiredPosition = (player.position + objectB.position) / 2;  // Midpoint between player and Object B
        }
        else if (!isTargeting && targetPoint != null)
        {
            // If not targeting, move the target point back to the player's local origin (0, 0, 0)
            desiredPosition = player.position;  // Return the target point to (0, 0, 0) relative to the player
        }
    }

    private void TryStartZTargeting()
    {
        if (player == null || targetPoint == null) return;

        // Find the nearest enemy in the scene
        objectB = FindNearestEnemy();
        if (objectB == null)
        {
            Debug.Log("No enemy found within range.");
            return;  // No enemy found, do nothing
        }

        float distanceToTarget = Vector3.Distance(player.position, objectB.position);
        if (distanceToTarget <= maxDistance)
        {
            // If within range, start targeting
            isTargeting = true;
            desiredPosition = (player.position + objectB.position) / 2;  // Set initial target point position to line center
        }
        else
        {
            Debug.Log("Target too far: Cannot link to the nearest enemy.");
        }
    }

    private void StopZTargeting()
    {
        isTargeting = false;
        objectB = null;  // Clear the target
        desiredPosition = player.position;  // Move the target point back to (0, 0, 0) relative to the player
    }

    private Transform FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        Transform nearestEnemy = null;
        float minDistance = Mathf.Infinity;

        foreach (GameObject enemy in enemies)
        {
            float distance = Vector3.Distance(player.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestEnemy = enemy.transform;
            }
        }

        return nearestEnemy;
    }
}
