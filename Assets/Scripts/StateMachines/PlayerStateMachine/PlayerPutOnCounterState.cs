using UnityEngine;

public class PlayerPutOnCounterState : BaseState<PlayerStateMachine>
{
    public PlayerPutOnCounterState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public static PlayerMovingState Create(PlayerStateMachine stateMachine)
    {
        return new PlayerMovingState(
            stateMachine,
            new PlayerPutOnCounterState(stateMachine),
            stateMachine.Player.GetWorkingClearCounter());
    }

    public override void Enter()
    {
        Debug.Log($"Player {stateMachine.Player.name} entered Put On Counter State");

        GameInput.Instance.Interact();

        stateMachine.ChangeState(new PlayerRecipePlanState(stateMachine));
    }
}
