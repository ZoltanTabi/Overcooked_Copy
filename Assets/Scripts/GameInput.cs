using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class GameInput : MonoBehaviour
{
    private PlayerInputActions playerInputActions;

    private Vector2 inputVector;
    private Vector2 normalizedInputVector;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();

        playerInputActions.Player.Enable();

        playerInputActions.Player.Move.performed += OnMove;
        playerInputActions.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Move.performed -= OnMove;
        playerInputActions.Player.Move.canceled -= OnMoveCanceled;

        playerInputActions.Player.Disable();
    }

    public Vector2 GetMovementVectorNormalized()
    {
        return normalizedInputVector;
    }

    private void OnMove(CallbackContext context)
    {
        inputVector = context.ReadValue<Vector2>();
        normalizedInputVector = inputVector.normalized;
    }

    private void OnMoveCanceled(CallbackContext context)
    {
        inputVector = Vector2.zero;
        normalizedInputVector = Vector2.zero;
    }
}
