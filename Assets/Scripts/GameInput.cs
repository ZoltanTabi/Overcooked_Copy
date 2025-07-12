using System;
using System.Collections;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class GameInput : MonoBehaviour
{
    private const float INPUT_HOLD_TIME = 0.6f;

    private PlayerInputActions playerInputActions;

    private Vector2 inputVector;
    private Vector2 normalizedInputVector;

    private bool interactAlternateHeld = false;
    private Coroutine interactAlternateHeldCoroutine;

    public event Action OnInteractAction;
    public event Action OnInteractAlternateAction;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions();

        playerInputActions.Player.Enable();

        playerInputActions.Player.Move.performed += OnMove;
        playerInputActions.Player.Move.canceled += OnMoveCanceled;

        playerInputActions.Player.Interact.performed += Interact_performed;

        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.InteractAlternate.canceled += InteractAlternate_canceled;
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
    
    private void Interact_performed(CallbackContext context)
    {
        OnInteractAction?.Invoke();
    }

    private void InteractAlternate_performed(CallbackContext context)
    {
        if (!interactAlternateHeld)
        {
            interactAlternateHeld = true;
            interactAlternateHeldCoroutine = StartCoroutine(FireInteractAlternateRepeatedly());
        }
    }

    private void InteractAlternate_canceled(CallbackContext context)
    {
        interactAlternateHeld = false;

        if (interactAlternateHeldCoroutine != null)
        {
            StopCoroutine(interactAlternateHeldCoroutine);
            interactAlternateHeldCoroutine = null;
        }
    }

    private IEnumerator FireInteractAlternateRepeatedly()
    {
        while (interactAlternateHeld)
        {
            OnInteractAlternateAction?.Invoke();
            yield return new WaitForSeconds(INPUT_HOLD_TIME);
        }
    }
}
