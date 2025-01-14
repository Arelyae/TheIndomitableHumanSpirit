using UnityEngine;
using Cinemachine;

public class CameraOrbitRadius : MonoBehaviour
{
    [SerializeField] private CinemachineFreeLook freeLookCamera;  // Reference to the FreeLook Camera
    [SerializeField] private Transform player;  // Reference to the player
    [SerializeField] private Transform targetPoint;  // The point used for Z-targeting
    [SerializeField] private float defaultRadius = 5f;  // Default orbit radius during normal gameplay
    [SerializeField] private float customRadius = 8f;  // Custom radius for normal gameplay
    [SerializeField] private float offsetMultiplier = 1.0f;  // Multiplier for the orbit radius when targeting
    [SerializeField] private float centerThreshold = 0.1f;  // Threshold for detecting if the target point is at the player's center
    [SerializeField] private float smoothTimeZoom = 0.5f;  // Time it takes to smoothly change the radius during zoom
    [SerializeField] private float smoothTimeDezoom = 0.5f;  // Time it takes to smoothly change the radius during de-zoom

    private float currentRadiusTop;  // Smoothly changing radius for top rig
    private float currentRadiusMiddle;  // Smoothly changing radius for middle rig
    private float currentRadiusBottom;  // Smoothly changing radius for bottom rig
    private float velocityTop;  // Internal velocity for `SmoothDamp` on top rig
    private float velocityMiddle;  // Internal velocity for `SmoothDamp` on middle rig
    private float velocityBottom;  // Internal velocity for `SmoothDamp` on bottom rig
    private bool isTargeting = false;  // Determines whether the camera is in targeting mode

    private void Start()
    {
        // Initialize current radii to the custom radius for normal gameplay
        currentRadiusTop = customRadius;
        currentRadiusMiddle = customRadius;
        currentRadiusBottom = customRadius;
        SetOrbitRadius(customRadius);  // Ensure the camera starts with the custom radius
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            isTargeting = true;  // Start targeting mode
        }

        if (Input.GetKeyUp(KeyCode.T))
        {
            isTargeting = false;  // Exit targeting mode
        }

        if (isTargeting)
        {
            if (IsTargetPointAtPlayerCenter())
            {
                SmoothResetOrbitRadius();  // Smoothly transition to default radius (custom gameplay radius)
            }
            else
            {
                SmoothUpdateOrbitRadius();  // Smoothly transition to dynamic radius during targeting
            }
        }
        else
        {
            SmoothSetCustomRadius();  // Keep the custom radius for normal gameplay when T is not pressed
        }
    }

    private bool IsTargetPointAtPlayerCenter()
    {
        return Vector3.Distance(targetPoint.position, player.position) <= centerThreshold;
    }

    private void SmoothUpdateOrbitRadius()
    {
        float distanceToTarget = Vector3.Distance(player.position, targetPoint.position);
        float targetRadius = distanceToTarget * offsetMultiplier;

        // Smoothly interpolate to the target radius during targeting
        currentRadiusTop = Mathf.SmoothDamp(currentRadiusTop, targetRadius, ref velocityTop, smoothTimeDezoom);
        currentRadiusMiddle = Mathf.SmoothDamp(currentRadiusMiddle, targetRadius, ref velocityMiddle, smoothTimeDezoom);
        currentRadiusBottom = Mathf.SmoothDamp(currentRadiusBottom, targetRadius, ref velocityBottom, smoothTimeDezoom);

        // Apply the smoothly changing radii
        SetOrbitRadius(currentRadiusTop);
    }

    private void SmoothResetOrbitRadius()
    {
        // Smoothly transition back to the default radius during targeting if no target is found
        currentRadiusTop = Mathf.SmoothDamp(currentRadiusTop, defaultRadius, ref velocityTop, smoothTimeZoom);
        currentRadiusMiddle = Mathf.SmoothDamp(currentRadiusMiddle, defaultRadius, ref velocityMiddle, smoothTimeZoom);
        currentRadiusBottom = Mathf.SmoothDamp(currentRadiusBottom, defaultRadius, ref velocityBottom, smoothTimeZoom);

        // Apply the smoothly changing radii
        SetOrbitRadius(currentRadiusTop);
    }

    private void SmoothSetCustomRadius()
    {
        // Smoothly set the custom radius during normal gameplay
        currentRadiusTop = Mathf.SmoothDamp(currentRadiusTop, customRadius, ref velocityTop, smoothTimeZoom);
        currentRadiusMiddle = Mathf.SmoothDamp(currentRadiusMiddle, customRadius, ref velocityMiddle, smoothTimeZoom);
        currentRadiusBottom = Mathf.SmoothDamp(currentRadiusBottom, customRadius, ref velocityBottom, smoothTimeZoom);

        // Apply the custom radii to all rigs
        SetOrbitRadius(currentRadiusTop);
    }

    private void SetOrbitRadius(float radius)
    {
        freeLookCamera.m_Orbits[0].m_Radius = radius;  // Top rig
        freeLookCamera.m_Orbits[1].m_Radius = radius;  // Middle rig
        freeLookCamera.m_Orbits[2].m_Radius = radius;  // Bottom rig
    }
}
