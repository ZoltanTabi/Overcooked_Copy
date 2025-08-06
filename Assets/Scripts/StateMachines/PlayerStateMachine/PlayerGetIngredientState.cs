using UnityEngine;

public class PlayerGetIngredientState : BaseState<PlayerStateMachine>
{
    private readonly BaseState<PlayerStateMachine> nextState = null;

    public PlayerGetIngredientState(PlayerStateMachine stateMachine, BaseState<PlayerStateMachine> nextState) : base(stateMachine)
    {
        this.nextState = nextState;
    }

    public static PlayerMovingState Create(PlayerStateMachine stateMachine, BaseState<PlayerStateMachine> nextState, KitchenObjectSO kitchenObjectSO)
    {
        return new PlayerMovingState(
            stateMachine,
            new PlayerGetIngredientState(stateMachine, nextState),
            stateMachine.GetNearestContainerCounterByIngredient(kitchenObjectSO));
    }

    public override void Enter()
    {
        Debug.Log($"Player {stateMachine.Player.name} entered Get Ingredient State");

        GameInput.Instance.Interact();

        stateMachine.ChangeState(nextState);
    }
}
