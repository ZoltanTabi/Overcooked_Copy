using UnityEngine;

public class PlayerFryingState : BaseState<PlayerStateMachine>
{
    private readonly FryingRecipeSO fryingRecipeSO;
    private StoveCounter stoveCounter = null;

    private BaseCounter targetCounter = null;
    private Vector2 moveDirection = Vector2.zero;

    public PlayerFryingState(PlayerStateMachine stateMachine, FryingRecipeSO fryingRecipeSO) : base(stateMachine)
    {
        this.fryingRecipeSO = fryingRecipeSO;
    }

    public override void Enter()
    {
        Debug.Log($"Player {stateMachine.Player.name} entered Frying State for {fryingRecipeSO.name}");
        stateMachine.Player.OnCounterSelected += Player_OnCounterSelected;

        (targetCounter, moveDirection) = stateMachine.GetNearestContainerCounterAndDirectionByIngredient(fryingRecipeSO.input);

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
        stoveCounter.OnStateChanged -= StoveCounter_OnStateChanged;
        GameInput.Instance.Move(Vector2.zero);
    }

    private void Player_OnCounterSelected(BaseCounter selectedCounter)
    {
        Debug.Log($"Frying State selected counter: {selectedCounter.name}");

        if (selectedCounter == targetCounter)
        {
            moveDirection = Vector2.zero;
        }

        if (selectedCounter == targetCounter && selectedCounter is ContainerCounter)
        {
            targetCounter.Interact(stateMachine.Player);
            (targetCounter, moveDirection) = stateMachine.GetNearestCounterAndDirection<StoveCounter>();

            return;
        }

        if (selectedCounter == targetCounter && selectedCounter is StoveCounter)
        {
            targetCounter.Interact(stateMachine.Player);

            stoveCounter = targetCounter as StoveCounter;
            stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;

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

    private void StoveCounter_OnStateChanged(StoveCounter.StoveCounterState stoveCounterState)
    {
        targetCounter.Interact(stateMachine.Player);

        stoveCounter.OnStateChanged -= StoveCounter_OnStateChanged;

        targetCounter = stateMachine.Player.GetWorkingClearCounter();
        moveDirection = stateMachine.GetMoveDirection(targetCounter);
    }
}
