using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float moveSpeed = 5f;

    [SerializeField]
    private float rotateSpeed = 10f;

    [SerializeField]
    private GameInput gameInput;

    private bool isWalking = false;

    private float MoveDistance => moveSpeed * Time.deltaTime;

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
    }

    public bool IsWalking()
    {
        return isWalking;
    }

    private Vector3 GetMoveDirection()
    {
        var inputVector = gameInput.GetMovementVectorNormalized();
        var moveDirection = new Vector3(inputVector.x, 0, inputVector.y);

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
}
