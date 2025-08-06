using UnityEngine;

public class PlayerPutOnStoveState : BaseState<PlayerStateMachine>
{
    public PlayerPutOnStoveState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public static PlayerMovingState Create(PlayerStateMachine stateMachine)
    {
        return new PlayerMovingState(stateMachine,
            new PlayerPutOnStoveState(stateMachine),
            stateMachine.GetNearestCounter<StoveCounter>());
    }

    public override void Enter()
    {
        Debug.Log($"Player {stateMachine.Player.name} entered Put On Stove State");

        if (stateMachine.Player.GetSelectedCounter() is not StoveCounter)
        {
            Debug.LogError("Selected counter is not a StoveCounter!");
            return;
        }

        GameInput.Instance.Interact();

        var stoveCounter = stateMachine.Player.GetSelectedCounter() as StoveCounter;
        stateMachine.PutIngredientOnStove(stoveCounter);

        stateMachine.ChangeState(new PlayerRecipePlanState(stateMachine));
    }
}
