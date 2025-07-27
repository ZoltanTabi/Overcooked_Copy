using UnityEngine;

public class PlayerGetIngredientState : BaseState<PlayerStateMachine>
{
    private readonly KitchenObjectSO neededIngredient;

    BaseCounter targetCounter = null;
    private Vector2 moveDirection = Vector2.zero;

    public PlayerGetIngredientState(PlayerStateMachine stateMachine, KitchenObjectSO neededIngredient) : base(stateMachine)
    {
        this.neededIngredient = neededIngredient;
    }

    public override void Enter()
    {
        Debug.Log($"Player {stateMachine.Player.name} entered Get Ingredient State for {neededIngredient.name}");
        stateMachine.Player.OnCounterSelected += Player_OnCounterSelected;

        (targetCounter, moveDirection) = stateMachine.GetNearestContainerCounterAndDirectionByIngredient(neededIngredient);

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
        Debug.Log($"Get Ingredient State selected counter: {selectedCounter.name}");

        if (selectedCounter == targetCounter)
        {
            moveDirection = Vector2.zero;
        }

        if (selectedCounter == targetCounter && selectedCounter is ContainerCounter)
        {
            targetCounter.Interact(stateMachine.Player);

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
