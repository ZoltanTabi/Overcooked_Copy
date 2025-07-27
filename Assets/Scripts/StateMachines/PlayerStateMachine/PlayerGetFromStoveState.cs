using UnityEngine;

public class PlayerGetFromStoveState : BaseState<PlayerStateMachine>
{
    public PlayerGetFromStoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public static PlayerMovingState Create(PlayerStateMachine stateMachine)
    {
        return new PlayerMovingState(
            stateMachine,
            new PlayerGetFromStoveState(stateMachine),
            stateMachine.StoveCounter);
    }

    public override void Enter()
    {
        Debug.Log($"Player {stateMachine.Player.name} entered Get From Stove State");

        GameInput.Instance.Interact();

        stateMachine.GetIngredientFromStove();

        stateMachine.ChangeState(PlayerPlaceOnPlateState.Create(stateMachine));
    }
}
