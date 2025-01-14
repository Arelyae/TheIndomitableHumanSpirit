using UnityEngine;
using UnityEngine.InputSystem;

public class ZTargeting : MonoBehaviour
{
    public Transform player;
    public Transform targetPoint;
    public float smoothSpeed = 5f;
    public float maxDistance = 20f;

    private Transform objectB;
    private Vector3 desiredPosition;
    private bool isTargeting = false;

    private void Update()
    {
        if (Gamepad.current.leftTrigger.isPressed || Keyboard.current.tKey.isPressed)
        {
            TryStartZTargeting();
        }

        if (Gamepad.current.leftTrigger.wasReleasedThisFrame || Keyboard.current.tKey.wasReleasedThisFrame)
        {
            StopZTargeting();
        }

        if (isTargeting && player != null && objectB != null)
        {
            float currentDistance = Vector3.Distance(player.position, objectB.position);
            if (currentDistance > maxDistance)
            {
                StopZTargeting();
                return;
            }
            desiredPosition = (player.position + objectB.position) / 2;
        }
        else if (!isTargeting && targetPoint != null)
        {
            desiredPosition = player.position;
        }

        if (targetPoint != null)
        {
            targetPoint.position = Vector3.Lerp(targetPoint.position, desiredPosition, Time.deltaTime * smoothSpeed);
        }
    }

    private void TryStartZTargeting()
    {
        objectB = FindNearestEnemy();
        if (objectB == null) return;

        if (Vector3.Distance(player.position, objectB.position) <= maxDistance)
        {
            isTargeting = true;
            desiredPosition = (player.position + objectB.position) / 2;
        }
    }

    private void StopZTargeting()
    {
        isTargeting = false;
        objectB = null;
        desiredPosition = player.position;
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
