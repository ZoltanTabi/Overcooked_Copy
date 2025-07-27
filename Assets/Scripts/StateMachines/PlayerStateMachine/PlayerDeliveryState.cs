using UnityEngine;

public class PlayerDeliveryState : BaseState<PlayerStateMachine>
{
    BaseCounter targetCounter = null;
    private Vector2 moveDirection = Vector2.zero;

    public PlayerDeliveryState(PlayerStateMachine stateMachine) : base(stateMachine)
    {
    }

    public override void Enter()
    {
        Debug.Log($"Player {stateMachine.Player.name} entered Delivery State");
        stateMachine.Player.OnCounterSelected += Player_OnCounterSelected;

        targetCounter = stateMachine.Player.GetWorkingClearCounter();
        moveDirection = stateMachine.GetMoveDirection(targetCounter);

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
        DeliveryManager.Instance.OnRecipeDelivered -= DeliveryManager_OnRecipeDelivered;
        GameInput.Instance.Move(Vector2.zero);
    }

    private void Player_OnCounterSelected(BaseCounter selectedCounter)
    {
        Debug.Log($"Delivery State selected counter: {selectedCounter.name}");

        if (selectedCounter == targetCounter)
        {
            moveDirection = Vector2.zero;
        }

        if (selectedCounter == targetCounter && selectedCounter is ClearCounter)
        {
            targetCounter.Interact(stateMachine.Player);

            (targetCounter, moveDirection) = stateMachine.GetNearestCounterAndDirection<DeliveryCounter>();

            return;
        }

        if (selectedCounter == targetCounter && selectedCounter is DeliveryCounter)
        {
            targetCounter.Interact(stateMachine.Player);

            DeliveryManager.Instance.OnRecipeDelivered += DeliveryManager_OnRecipeDelivered;

            return;
        }

        moveDirection = stateMachine.GetMoveDirection(targetCounter);
    }

    private void DeliveryManager_OnRecipeDelivered()
    {
        DeliveryManager.Instance.OnRecipeDelivered -= DeliveryManager_OnRecipeDelivered;

        stateMachine.SetCurrentRecipe(null);
        stateMachine.ChangeState(new PlayerIdleState(stateMachine));
    }
}
