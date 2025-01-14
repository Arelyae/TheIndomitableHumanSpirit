using UnityEngine;

public class ControllerDebug : MonoBehaviour
{
    void Update()
    {
        Debug.Log($"RightJoystick X: {Input.GetAxis("RightJoystick X")}, RightJoystick Y: {Input.GetAxis("RightJoystick Y")}");
    }
}
