using UnityEngine;

public class PlayerGetPlateState : BaseState<PlayerStateMachine>
{
    BaseCounter targetCounter = null;
    private Vector2 moveDirection = Vector2.zero;

    public PlayerGetPlateState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log($"Player {stateMachine.Player.name} entered Get Plate State");
        stateMachine.Player.OnCounterSelected += Player_OnCounterSelected;

        (targetCounter, moveDirection) = stateMachine.GetNearestCounterAndDirection<PlatesCounter>();

        if (targetCounter == stateMachine.Player.GetSelectedCounter())
        {
            Player_OnCounterSelected(stateMachine.Player.GetSelectedCounter());
        }
    }

    public override void Update()
    {
        if (moveDirection != Vector2.zero)
        {
            moveDirection = stateMachine.GetMoveDirection(targetCounter);
        }

        GameInput.Instance.Move(moveDirection);
        GameInput.Instance.Dash();
    }

    public override void Exit()
    {
        stateMachine.Player.OnCounterSelected -= Player_OnCounterSelected;
        GameInput.Instance.Move(Vector2.zero);
    }

    private void Player_OnCounterSelected(BaseCounter selectedCounter)
    {
        Debug.Log($"Get Plate State selected counter: {selectedCounter.name}");

        if (selectedCounter == targetCounter)
        {
            moveDirection = Vector2.zero;
        }

        if (selectedCounter == targetCounter && selectedCounter is PlatesCounter)
        {
            targetCounter.Interact(stateMachine.Player);

            if (stateMachine.Player.GetKitchenObject().TryGetPlate(out var plateKitchenObject))
            {
                stateMachine.SetPlateKitchenObject(plateKitchenObject);
            }
            else
            {
                Debug.LogError("PlayerGetPlateState: Player does not have a PlateKitchenObject after interacting with PlatesCounter.");
            }

            targetCounter = stateMachine.Player.GetWorkingClearCounter();
            moveDirection = stateMachine.GetMoveDirection(targetCounter);

            return;
        }

        if (selectedCounter == targetCounter && selectedCounter is ClearCounter)
        {
            targetCounter.Interact(stateMachine.Player);

            stateMachine.ChangeState(new PlayerRecipePlanState(stateMachine));

            return;
        }

        moveDirection = stateMachine.GetMoveDirection(targetCounter);
    }
}
