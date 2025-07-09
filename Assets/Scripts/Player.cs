using System;
using UnityEngine;

public class Player : MonoBehaviour, IKitchenObjectParent
{
    public static Player Instance { get; private set; }

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotateSpeed = 10f;
    [SerializeField] private GameInput gameInput;
    [SerializeField] private LayerMask counterLayerMask;
    [SerializeField] private Transform kitchenObjectHoldPoint;

    private bool isWalking = false;
    private Vector3 lastInteractDirection = Vector3.zero;
    private BaseCounter selectedCounter;
    private KitchenObject kitchenObject;

    private float MoveDistance => moveSpeed * Time.deltaTime;

    public event Action<BaseCounter> OnCounterSelected;

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
    }

    private void Update()
    {
        Vector3 moveDirection = GetMoveDirection();

        if (moveDirection != Vector3.zero)
        {
            isWalking = true;

            Move(moveDirection);
            Rotate(moveDirection);
        }
        else
        {
            isWalking = false;
        }

        HandleInteractions();
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

        float playerRadius = 0.7f; // Adjust based on your player model size
        float playerHeight = 2f; // Adjust based on your player model height
        bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirection, MoveDistance);

        if (!canMove)
        {
            var moveDirectionX = new Vector3(moveDirection.x, 0, 0).normalized;
            canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionX, MoveDistance);

            if (canMove)
            {
                moveDirection = moveDirectionX;
            }
            else
            {
                var moveDirectionZ = new Vector3(0, 0, moveDirection.z).normalized;
                canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirectionZ, MoveDistance);

                if (canMove)
                {
                    moveDirection = moveDirectionZ;
                }
            }
        }

        return canMove ? moveDirection : Vector3.zero;
    }

    private void Move(Vector3 moveDirection)
    {
        transform.position += MoveDistance * moveDirection;
    }

    private void Rotate(Vector3 moveDirection)
    {
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
    }

    private void HandleInteractions()
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
        if (selectedCounter != null)
        {
            selectedCounter.Interact(this);
        }
    }

    public Transform GetKitchenObjectFollowTransform()
    {
        return kitchenObjectHoldPoint;
    }

    public void SetKitchenObject(KitchenObject kitchenObject)
    {
        this.kitchenObject = kitchenObject;
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
