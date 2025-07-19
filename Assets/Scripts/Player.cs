using System;
using System.Collections;
using System.Linq;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    private const float PLAYER_RADIUS = 0.7f;
    private const float PLAYER_HEIGHT = 2f;
    private const float CONTROLLER_DRIFTING_ERROR_MARGIN = 0.35f;
    private const float DASH_COOLDOWN_TIME = .6f;

    public static Player Instance { get; private set; }

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private float dashMultiplier = 3f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking = false;
    private bool isDashing = false;
    private bool isDashCooldown = false;
    public float dashDurationTimer = 0f;
    public float dashDurationTimerMax = 0.2f;
    private Vector3 lastInteractDirection = Vector3.zero;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private float MoveDistance => moveSpeed * Time.deltaTime;

    public event Action<BaseCounter> OnCounterSelected;
    public event Action OnPickSomething;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        gameInput.OnInteractAction += GameInput_OnInteractAction;
        gameInput.OnInteractAlternateAction += GameInput_OnInteractAlternateAction;
        gameInput.OnDashAction += GameInput_OnDashAction;
    }

    private void Update()
    {
        if (isDashing)
        {
            dashDurationTimer += Time.deltaTime;

            if (dashDurationTimer >= dashDurationTimerMax)
            {
                isDashing = false;
                dashDurationTimer = 0f;
            }
            else
            {
                Dash();

                return;
            }
        }

        Vector3 moveDirection = GetMoveDirection();

        if (moveDirection != Vector3.zero)
        {
            isWalking = true;

            Move(moveDirection);
        }
        else
        {
            isWalking = false;
        }

        Rotate();
        HandleAvailableInteractions();
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private Vector3 GetMoveDirection()
    {
        var inputVector = gameInput.GetMovementVectorNormalized();
        var moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

        if (moveDirection != Vector3.zero)
        {
            lastInteractDirection = moveDirection.normalized;
        }

        Vector3[] directions = {
            moveDirection,
            new Vector3(moveDirection.x, 0, 0).normalized,
            new Vector3(0, 0, moveDirection.z).normalized
        };

        foreach (var (direction, i) in directions.Select((x, i) => (x, i)))
        {
            if (direction == Vector3.zero)
            {
                continue;
            }

            bool canMove = (i == 0
                    || i == 1 && (moveDirection.x < -CONTROLLER_DRIFTING_ERROR_MARGIN || moveDirection.x > +CONTROLLER_DRIFTING_ERROR_MARGIN)
                    || i == 2 && (moveDirection.z < -CONTROLLER_DRIFTING_ERROR_MARGIN || moveDirection.z > +CONTROLLER_DRIFTING_ERROR_MARGIN)
                ) && CanMoveThatDirection(direction, MoveDistance);

            if (canMove)
            {
                return direction;
            }
        }

        return Vector3.zero;
    }

    private void Move(Vector3 moveDirection)
    {
        transform.position += MoveDistance * moveDirection;
    }

    private void Dash()
    {
        if (lastInteractDirection == Vector3.zero)
        {
            return;
        }

        if (CanMoveThatDirection(lastInteractDirection, MoveDistance * dashMultiplier))
        {
            Move(lastInteractDirection * dashMultiplier);

            return;
        }

        if (CanMoveThatDirection(lastInteractDirection, MoveDistance))
        {
            Move(lastInteractDirection);

            return;
        }

        isDashing = false;
    }

    private void Rotate()
    {
        if (lastInteractDirection == Vector3.zero)
        {
            return;
        }

        transform.forward = Vector3.Slerp(transform.forward, lastInteractDirection, rotateSpeed * Time.deltaTime);
    }

    private bool CanMoveThatDirection(Vector3 direction, float maxDistance)
    {
        return !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * PLAYER_HEIGHT, PLAYER_RADIUS, direction, maxDistance);
    }

    private void HandleAvailableInteractions()
    {
        float interactDistance = 2f;

        if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit hit, interactDistance, counterLayerMask))
        {
            if (hit.transform.TryGetComponent(out BaseCounter baseCounter))
            {
                SetSelectedCounter(baseCounter);
            }
            else
            {
                SetSelectedCounter(null);
            }
        }
        else
        {
            SetSelectedCounter(null);
        }
    }

    private void SetSelectedCounter(BaseCounter selectedCounter)
    {
        if (this.selectedCounter == selectedCounter)
        {
            return;
        }

        this.selectedCounter = selectedCounter;

        OnCounterSelected?.Invoke(selectedCounter);
    }

    private void GameInput_OnInteractAction()
    {
        if (!GameManager.Instance.IsGamePlaying())
        {
            return;
        }

        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    private void GameInput_OnInteractAlternateAction()
    {
        if (!GameManager.Instance.IsGamePlaying())
        {
            return;
        }

        if (selectedCounter != null)
        {
            selectedCounter.InteractAlternate(this);
        }
    }

    private void GameInput_OnDashAction()
    {
        if (lastInteractDirection == Vector3.zero || isDashCooldown)
        {
            return;
        }

        isDashing = true;
        StartCoroutine(DashCooldownRoutine());
    }

    private IEnumerator DashCooldownRoutine()
    {
        isDashCooldown = true;
        yield return new WaitForSeconds(DASH_COOLDOWN_TIME);
        isDashCooldown = false;
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;

        if (kitchenObject != null)
        {
            OnPickSomething?.Invoke();
        }
    }

    public KitchenObject GetKitchenObject()
    {
        return kitchenObject;
    }

    public void ClearKitchenObject()
    {
        kitchenObject = null;
    }

    public bool HasKitchenObject()
    {
        return kitchenObject != null;
    }
}
