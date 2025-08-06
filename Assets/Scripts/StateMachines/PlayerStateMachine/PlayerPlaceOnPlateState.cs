using UnityEngine;

public class PlayerPlaceOnPlateState : BaseState<PlayerStateMachine>
{
    public PlayerPlaceOnPlateState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public static PlayerMovingState Create(PlayerStateMachine stateMachine)
    {
        return new PlayerMovingState(
            stateMachine,
            new PlayerPlaceOnPlateState(stateMachine),
            stateMachine.Player.GetWorkingClearCounter());
    }

    public override void Enter()
    {
        Debug.Log($"Player {stateMachine.Player.name} entered Place On Plate State");

        GameInput.Instance.Interact();

        stateMachine.ChangeState(new PlayerRecipePlanState(stateMachine));
    }
}
