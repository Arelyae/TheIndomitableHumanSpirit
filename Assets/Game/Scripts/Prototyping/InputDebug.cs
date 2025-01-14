using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputDebug : MonoBehaviour
{
    private void Update()
    {
        // Check if Fire2 is being pressed
        if (Input.GetButton("Target"))
        {
            Debug.Log("Target (L2) is being held down.");
        }

        // Check if Fire2 was just pressed
        if (Input.GetButtonDown("Target"))
        {
            Debug.Log("Target (L2) was pressed.");
        }

        // Check if Fire2 was just released
        if (Input.GetButtonUp("Target"))
        {
            Debug.Log("Target (L2) was released.");
        }

    }
}
