using UnityEngine;

public class CustomCharacterController : MonoBehaviour
{
    private InputHandler inputHandler;
    private MovementHandler movementHandler;

    private void Awake()
    {
        inputHandler = GetComponent<InputHandler>();
        movementHandler = GetComponent<MovementHandler>();
    }

    private void Update()
    {
        Vector3 movementInput = inputHandler.GetMovementInput();
        movementHandler.Rotate(movementInput);
    }

    private void FixedUpdate()
    {
        Vector3 movementInput = inputHandler.GetMovementInput();
        movementHandler.Move(movementInput);
    }
}
