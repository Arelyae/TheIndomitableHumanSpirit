using UnityEngine;

public class InputHandler : MonoBehaviour
{
    public Vector3 GetMovementInput()
    {
        float horizontal = Input.GetAxis("Horizontal");  // Left Stick X
        float vertical = Input.GetAxis("Vertical");      // Left Stick Y
        return new Vector3(horizontal, 0f, vertical).normalized;
    }

    public Vector2 GetLookInput()
    {
        float lookHorizontal = Input.GetAxis("RightStickHorizontal");  // Right Stick X
        float lookVertical = Input.GetAxis("RightStickVertical");      // Right Stick Y
        return new Vector2(lookHorizontal, lookVertical);
    }
}
