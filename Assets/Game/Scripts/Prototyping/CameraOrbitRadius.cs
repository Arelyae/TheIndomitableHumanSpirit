using UnityEngine;
using Cinemachine;

public class CameraOrbitRadius : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook freeLookCamera;  // Reference to the FreeLook Camera
    [SerializeField] private Transform player;  // Reference to the player
    [SerializeField] private Transform targetPoint;  // The point used for Z-targeting
    [SerializeField] private float defaultRadius = 5f;  // Default orbit radius
    [SerializeField] private float offsetMultiplier = 1.0f;  // Multiplier for the orbit radius
    [SerializeField] private float centerThreshold = 0.1f;  // Threshold for detecting if the target point is at the player's center

    private void Update()
    {
        if (IsTargetPointAtPlayerCenter())
        {
            ResetOrbitRadius();  // Keep the default radius if there is no valid target
        }
        else
        {
            UpdateOrbitRadius();  // Adjust radius based on distance
        }
    }

    private bool IsTargetPointAtPlayerCenter()
    {
        return Vector3.Distance(targetPoint.position, player.position) <= centerThreshold;
    }

    private void UpdateOrbitRadius()
    {
        float distanceToTarget = Vector3.Distance(player.position, targetPoint.position);
        float adjustedRadius = distanceToTarget * offsetMultiplier;
        SetOrbitRadius(adjustedRadius);
    }

    private void ResetOrbitRadius()
    {
        SetOrbitRadius(defaultRadius);
    }

    private void SetOrbitRadius(float radius)
    {
        for (int i = 0; i < freeLookCamera.m_Orbits.Length; i++)
        {
            freeLookCamera.m_Orbits[i].m_Radius = radius;
        }
    }
}
