using UnityEngine;
using Cinemachine;
using UnityEngine.InputSystem;

public class CameraOrbitRadius : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CinemachineFreeLook freeLookCamera;  // Reference to the Cinemachine FreeLook Camera
    [SerializeField] private Transform player;  // Reference to the player
    [SerializeField] private Transform targetPoint;  // The point used for Z-targeting

    [Header("Default Camera Settings (Normal Gameplay)")]
    [SerializeField] private float defaultTopRadius = 5f;
    [SerializeField] private float defaultMiddleRadius = 5f;
    [SerializeField] private float defaultBottomRadius = 5f;
    [SerializeField] private float defaultTopHeight = 8f;
    [SerializeField] private float defaultMiddleHeight = 4f;
    [SerializeField] private float defaultBottomHeight = 0f;

    [Header("Z-Targeting Settings")]
    [SerializeField] private float targetRadiusOffset = 2f;  // Offset added to the radius during Z-targeting
    [SerializeField] private float zTargetingFOV = 30f;  // FOV during Z-targeting
    [SerializeField] private float defaultFOV = 50f;  // Default FOV during normal gameplay

    [Header("Z-Targeting Heights")]
    [SerializeField] private float zTargetingTopHeight = 6f;  // Height for the top ring during Z-targeting
    [SerializeField] private float zTargetingMiddleHeight = 3f;  // Height for the middle ring during Z-targeting
    [SerializeField] private float zTargetingBottomHeight = 0f;  // Height for the bottom ring during Z-targeting

    [Header("Smoothing Settings")]
    [SerializeField] private float smoothTimeZoom = 0.5f;  // Time to smoothly adjust the camera zoom
    [SerializeField] private float smoothTimeDezoom = 0.5f;  // Time to smoothly reset the camera
    [SerializeField] private float fovSmoothingTime = 0.3f;  // Time to smoothly transition the FOV

    private float currentRadius;  // Used for SmoothDamp
    private float currentFOVVelocity = 0f;  // Velocity for FOV smooth transition
    private bool isTargeting = false;

    /// <summary>
    /// Initializes the current camera values with default gameplay settings.
    /// </summary>
    private void Start()
    {
        ApplyInspectorDefinedValues();  // Initialize the camera to default gameplay settings
        freeLookCamera.m_Lens.FieldOfView = defaultFOV;  // Set default FOV
    }

    /// <summary>
    /// Handles input and determines whether the camera should adjust for Z-targeting.
    /// </summary>
    private void Update()
    {
        // Check for Z-targeting input (T for keyboard, L2 for controller)
        if (Keyboard.current.tKey.wasPressedThisFrame || Gamepad.current.leftTrigger.wasPressedThisFrame)
        {
            isTargeting = true;
        }

        if (Keyboard.current.tKey.wasReleasedThisFrame || Gamepad.current.leftTrigger.wasReleasedThisFrame)
        {
            isTargeting = false;
        }

        if (isTargeting)
        {
            AdjustCameraForTargeting();
        }
        else
        {
            ApplyInspectorDefinedValues();
        }
    }

    /// <summary>
    /// Adjusts the camera radii and heights during Z-targeting.
    /// </summary>
    private void AdjustCameraForTargeting()
    {
        float distanceToTarget = Vector3.Distance(player.position, targetPoint.position);

        // All radii become equal to the distance between the player and the target plus offset
        float targetRadius = distanceToTarget + targetRadiusOffset;

        // Smoothly update the radii for all three rings
        currentRadius = Mathf.SmoothDamp(currentRadius, targetRadius, ref smoothTimeZoom, smoothTimeDezoom);

        // Apply inspector-defined heights for Z-targeting
        SetOrbitValues(currentRadius, currentRadius, currentRadius, zTargetingTopHeight, zTargetingMiddleHeight, zTargetingBottomHeight);

        // Smoothly transition the FOV to Z-targeting FOV
        float currentFOV = freeLookCamera.m_Lens.FieldOfView;
        freeLookCamera.m_Lens.FieldOfView = Mathf.SmoothDamp(currentFOV, zTargetingFOV, ref currentFOVVelocity, fovSmoothingTime);
    }

    /// <summary>
    /// Applies the values as defined in the Inspector during normal gameplay.
    /// </summary>
    private void ApplyInspectorDefinedValues()
    {
        // Apply inspector-defined radii and heights directly
        SetOrbitValues(defaultTopRadius, defaultMiddleRadius, defaultBottomRadius, defaultTopHeight, defaultMiddleHeight, defaultBottomHeight);

        // Smoothly transition the FOV back to default FOV
        float currentFOV = freeLookCamera.m_Lens.FieldOfView;
        freeLookCamera.m_Lens.FieldOfView = Mathf.SmoothDamp(currentFOV, defaultFOV, ref currentFOVVelocity, fovSmoothingTime);
    }

    /// <summary>
    /// Sets the orbit radii and heights for the top, middle, and bottom rings.
    /// </summary>
    private void SetOrbitValues(float topRadius, float middleRadius, float bottomRadius, float topHeight, float middleHeight, float bottomHeight)
    {
        freeLookCamera.m_Orbits[0].m_Radius = topRadius;  // Top ring
        freeLookCamera.m_Orbits[1].m_Radius = middleRadius;  // Middle ring
        freeLookCamera.m_Orbits[2].m_Radius = bottomRadius;  // Bottom ring

        freeLookCamera.m_Orbits[0].m_Height = topHeight;  // Top ring height
        freeLookCamera.m_Orbits[1].m_Height = middleHeight;  // Middle ring height
        freeLookCamera.m_Orbits[2].m_Height = bottomHeight;  // Bottom ring height
    }
}
