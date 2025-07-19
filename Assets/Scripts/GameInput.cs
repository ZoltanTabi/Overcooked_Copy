using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

public class GameInput : MonoBehaviour
{
    private const float INTERACT_ALTERNATE_FIRE_TIME = 0.3f;
    private const string PLAYER_PREFS_BINDING_KEY = "InputBindings";
    private const string ESCAPE_KEY = "Escape";
    private const string ESCAPE_KEY_DISPLAY_NAME = "esc";

    public enum Binding
    {
        Move_Up,
        Move_Down,
        Move_Left,
        Move_Right,
        Interact,
        InteractAlternate,
        Pause,
        Gamepad_Interact,
        Gamepad_InteractAlternate,
        Gamepad_Pause
    }

    public static GameInput Instance { get; private set; }

    private PlayerInputActions playerInputActions;

    private Vector2 inputVector;
    private Vector2 normalizedInputVector;

    private bool interactAlternateHeld = false;
    private Coroutine interactAlternateHeldCoroutine;

    public event Action OnInteractAction;
    public event Action OnInteractAlternateAction;
    public event Action OnDashAction;
    public event Action OnPauseAction;
    public event Action OnBindingRebind;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        playerInputActions = new PlayerInputActions();

        if (PlayerPrefs.HasKey(PLAYER_PREFS_BINDING_KEY))
        {
            playerInputActions.LoadBindingOverridesFromJson(PlayerPrefs.GetString(PLAYER_PREFS_BINDING_KEY));
        }

        playerInputActions.Player.Enable();

        playerInputActions.Player.Move.performed += OnMove;
        playerInputActions.Player.Move.canceled += OnMoveCanceled;

        playerInputActions.Player.Interact.performed += Interact_performed;

        playerInputActions.Player.InteractAlternate.performed += InteractAlternate_performed;
        playerInputActions.Player.InteractAlternate.canceled += InteractAlternate_canceled;

        playerInputActions.Player.Dash.performed += Dash_performed;

        playerInputActions.Player.Pause.performed += Pause_performed;
    }

    private void OnDestroy()
    {
        playerInputActions.Player.Move.performed -= OnMove;
        playerInputActions.Player.Move.canceled -= OnMoveCanceled;
        playerInputActions.Player.Interact.performed -= Interact_performed;
        playerInputActions.Player.InteractAlternate.performed -= InteractAlternate_performed;
        playerInputActions.Player.InteractAlternate.canceled -= InteractAlternate_canceled;
        playerInputActions.Player.Pause.performed -= Pause_performed;

        playerInputActions.Player.Disable();

        playerInputActions.Dispose();
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
            yield return new WaitForSeconds(INTERACT_ALTERNATE_FIRE_TIME);
        }
    }

    private void Dash_performed(CallbackContext context)
    {
        OnDashAction?.Invoke();
    }

    private void Pause_performed(CallbackContext context)
    {
        OnPauseAction?.Invoke();
    }

    public string GetBindingText(Binding binding)
    {
        var displayName = binding switch
        {
            Binding.Move_Up => playerInputActions.Player.Move.bindings[1].ToDisplayString(),
            Binding.Move_Down => playerInputActions.Player.Move.bindings[2].ToDisplayString(),
            Binding.Move_Left => playerInputActions.Player.Move.bindings[3].ToDisplayString(),
            Binding.Move_Right => playerInputActions.Player.Move.bindings[4].ToDisplayString(),
            Binding.Interact => playerInputActions.Player.Interact.bindings[0].ToDisplayString(),
            Binding.InteractAlternate => playerInputActions.Player.InteractAlternate.bindings[0].ToDisplayString(),
            Binding.Pause => playerInputActions.Player.Pause.bindings[0].ToDisplayString(),
            Binding.Gamepad_Interact => playerInputActions.Player.Interact.bindings[1].ToDisplayString(),
            Binding.Gamepad_InteractAlternate => playerInputActions.Player.InteractAlternate.bindings[1].ToDisplayString(),
            Binding.Gamepad_Pause => playerInputActions.Player.Pause.bindings[1].ToDisplayString(),
            _ => throw new ArgumentOutOfRangeException(nameof(binding), binding, null),
        };

        if (displayName.Equals(ESCAPE_KEY, StringComparison.OrdinalIgnoreCase))
        {
            displayName = ESCAPE_KEY_DISPLAY_NAME;
        }

        return displayName;
    }

    public void RebindBinding(Binding binding, Action onActionRebound)
    {
        playerInputActions.Player.Disable();

        var (inputAction, bindingIndex) = binding switch
        {
            Binding.Move_Up => (playerInputActions.Player.Move, 1),
            Binding.Move_Down => (playerInputActions.Player.Move, 2),
            Binding.Move_Left => (playerInputActions.Player.Move, 3),
            Binding.Move_Right => (playerInputActions.Player.Move, 4),
            Binding.Interact => (playerInputActions.Player.Interact, 0),
            Binding.InteractAlternate => (playerInputActions.Player.InteractAlternate, 0),
            Binding.Pause => (playerInputActions.Player.Pause, 0),
            Binding.Gamepad_Interact => (playerInputActions.Player.Interact, 1),
            Binding.Gamepad_InteractAlternate => (playerInputActions.Player.InteractAlternate, 1),
            Binding.Gamepad_Pause => (playerInputActions.Player.Pause, 1),
            _ => throw new ArgumentOutOfRangeException(nameof(binding), binding, null),
        };

        inputAction.PerformInteractiveRebinding(bindingIndex)
            .OnComplete((callback) =>
            {
                callback.Dispose();
                playerInputActions.Player.Enable();
                onActionRebound();

                PlayerPrefs.SetString(PLAYER_PREFS_BINDING_KEY, playerInputActions.SaveBindingOverridesAsJson());
                PlayerPrefs.Save();

                OnBindingRebind?.Invoke();
            })
            .Start();
    }
}
