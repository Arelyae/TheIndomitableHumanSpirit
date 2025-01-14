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

    [Header("Z-Targeting Camera Settings")]
    [SerializeField] private float customTopRadius = 10f;
    [SerializeField] private float customMiddleRadius = 9f;
    [SerializeField] private float customBottomRadius = 8f;
    [SerializeField] private float customTopHeight = 6f;
    [SerializeField] private float customMiddleHeight = 3f;
    [SerializeField] private float customBottomHeight = 0f;

    [Header("Smoothing Settings")]
    [SerializeField] private float smoothTimeZoom = 0.5f;  // Time to smoothly adjust the camera zoom
    [SerializeField] private float smoothTimeDezoom = 0.5f;  // Time to smoothly reset the camera

    private float currentTopRadius, currentMiddleRadius, currentBottomRadius;
    private float currentTopHeight, currentMiddleHeight, currentBottomHeight;
    private bool isTargeting = false;

    /// <summary>
    /// Initializes the current camera values with default gameplay settings.
    /// </summary>
    private void Start()
    {
        currentTopRadius = defaultTopRadius;
        currentMiddleRadius = defaultMiddleRadius;
        currentBottomRadius = defaultBottomRadius;

        currentTopHeight = defaultTopHeight;
        currentMiddleHeight = defaultMiddleHeight;
        currentBottomHeight = defaultBottomHeight;

        SetOrbitValues(defaultTopRadius, defaultMiddleRadius, defaultBottomRadius, defaultTopHeight, defaultMiddleHeight, defaultBottomHeight);
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
            ResetToDefaultOrbit();
        }
    }

    /// <summary>
    /// Adjusts the camera radii and heights to keep the player and target visible.
    /// </summary>
    private void AdjustCameraForTargeting()
    {
        float distanceToTarget = Vector3.Distance(player.position, targetPoint.position);

        currentTopRadius = Mathf.SmoothDamp(currentTopRadius, customTopRadius, ref smoothTimeZoom, smoothTimeDezoom);
        currentMiddleRadius = Mathf.SmoothDamp(currentMiddleRadius, customMiddleRadius, ref smoothTimeZoom, smoothTimeDezoom);
        currentBottomRadius = Mathf.SmoothDamp(currentBottomRadius, customBottomRadius, ref smoothTimeZoom, smoothTimeDezoom);

        currentTopHeight = Mathf.SmoothDamp(currentTopHeight, customTopHeight, ref smoothTimeZoom, smoothTimeDezoom);
        currentMiddleHeight = Mathf.SmoothDamp(currentMiddleHeight, customMiddleHeight, ref smoothTimeZoom, smoothTimeDezoom);
        currentBottomHeight = Mathf.SmoothDamp(currentBottomHeight, customBottomHeight, ref smoothTimeZoom, smoothTimeDezoom);

        SetOrbitValues(currentTopRadius, currentMiddleRadius, currentBottomRadius, currentTopHeight, currentMiddleHeight, currentBottomHeight);
    }

    /// <summary>
    /// Resets the camera to default gameplay radii and heights.
    /// </summary>
    private void ResetToDefaultOrbit()
    {
        currentTopRadius = Mathf.SmoothDamp(currentTopRadius, defaultTopRadius, ref smoothTimeZoom, smoothTimeDezoom);
        currentMiddleRadius = Mathf.SmoothDamp(currentMiddleRadius, defaultMiddleRadius, ref smoothTimeZoom, smoothTimeDezoom);
        currentBottomRadius = Mathf.SmoothDamp(currentBottomRadius, defaultBottomRadius, ref smoothTimeZoom, smoothTimeDezoom);

        currentTopHeight = Mathf.SmoothDamp(currentTopHeight, defaultTopHeight, ref smoothTimeZoom, smoothTimeDezoom);
        currentMiddleHeight = Mathf.SmoothDamp(currentMiddleHeight, defaultMiddleHeight, ref smoothTimeZoom, smoothTimeDezoom);
        currentBottomHeight = Mathf.SmoothDamp(currentBottomHeight, defaultBottomHeight, ref smoothTimeZoom, smoothTimeDezoom);

        SetOrbitValues(currentTopRadius, currentMiddleRadius, currentBottomRadius, currentTopHeight, currentMiddleHeight, currentBottomHeight);
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
