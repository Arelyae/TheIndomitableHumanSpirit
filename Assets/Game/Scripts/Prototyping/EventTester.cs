using UnityEngine;
using UnityEngine.Events;

public class EventTester : MonoBehaviour
{
    public UnityEvent<int> OnEventToTest;
    public int eventId;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            OnEventToTest.Invoke(eventId);
        }
    }
}