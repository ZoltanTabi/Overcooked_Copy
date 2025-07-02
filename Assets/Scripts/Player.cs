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

        return new Vector3(inputVector.x, 0, inputVector.y);
    }

    private void Move(Vector3 moveDirection)
    {
        transform.position += moveSpeed * Time.deltaTime * moveDirection;
    }

    private void Rotate(Vector3 moveDirection)
    {
        transform.forward = Vector3.Slerp(transform.forward, moveDirection, rotateSpeed * Time.deltaTime);
    }
}
