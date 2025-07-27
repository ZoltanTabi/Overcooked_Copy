using UnityEngine;

public class PlayerGetPlateState : BaseState<PlayerStateMachine>
{
    public PlayerGetPlateState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public static PlayerMovingState Create(PlayerStateMachine stateMachine)
    {
        return new PlayerMovingState(stateMachine,
            new PlayerGetPlateState(stateMachine),
            stateMachine.GetNearestCounter<PlatesCounter>());
    }

    public override void Enter()
    {
        Debug.Log($"Player {stateMachine.Player.name} entered Get Plate State");

        GameInput.Instance.Interact();

        if (stateMachine.Player.GetKitchenObject().TryGetPlate(out var plateKitchenObject))
        {
            stateMachine.SetPlateKitchenObject(plateKitchenObject);
        }
        else
        {
            Debug.LogError("PlayerGetPlateState: Player does not have a PlateKitchenObject after interacting with PlatesCounter.");

            return;
        }

        stateMachine.ChangeState(PlayerPutOnCounterState.Create(stateMachine));
    }
}
