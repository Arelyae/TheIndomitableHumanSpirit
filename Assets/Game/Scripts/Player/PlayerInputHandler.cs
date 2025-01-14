using UnityEngine;

public class InputHandler : MonoBehaviour
{
    private Vector3 movementInput = Vector3.zero;
    private bool lockOnPressed = false;

    // Get player movement input (used by MovementHandler)
    public Vector3 GetMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        movementInput = new Vector3(horizontal, 0f, vertical).normalized;
        return movementInput;
    }

    // Get lock-on input (used by ZTargetingHandler)
    public bool GetLockOnInput()
    {
        return Input.GetButtonDown("LockOn");  // Detect L2 press (mapped to "LockOn" in Input Manager)
    }
}
