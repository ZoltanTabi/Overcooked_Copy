using UnityEngine;

public class PlayerCuttingState : BaseState<PlayerStateMachine>
{
    private readonly CuttingRecipeSO cuttingRecipeSO;

    BaseCounter targetCounter = null;
    private Vector2 moveDirection = Vector2.zero;

    public PlayerCuttingState(PlayerStateMachine stateMachine, CuttingRecipeSO cuttingRecipeSO) : base(stateMachine)
    {
        this.cuttingRecipeSO = cuttingRecipeSO;
    }

    public override void Enter()
    {
        Debug.Log($"Player {stateMachine.Player.name} entered Cutting State for {cuttingRecipeSO.input.name}");
        stateMachine.Player.OnCounterSelected += Player_OnCounterSelected;

        (targetCounter, moveDirection) = stateMachine.GetNearestContainerCounterAndDirectionByIngredient(cuttingRecipeSO.input);

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
        Debug.Log($"Cutting State selected counter: {selectedCounter.name}");

        if (selectedCounter == targetCounter)
        {
            moveDirection = Vector2.zero;
        }

        if (selectedCounter == targetCounter && selectedCounter is ContainerCounter)
        {
            targetCounter.Interact(stateMachine.Player);
            (targetCounter, moveDirection) = stateMachine.GetNearestCounterAndDirection<CuttingCounter>();

            return;
        }

        if (selectedCounter == targetCounter && selectedCounter is CuttingCounter)
        {
            targetCounter.Interact(stateMachine.Player);

            for (var i = 0; i < cuttingRecipeSO.cuttingProgressMax; i++)
            {
                targetCounter.InteractAlternate(stateMachine.Player);
            }

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
