using UnityEngine;

public class PlayerDeliveryState : BaseState<PlayerStateMachine>
{
    public PlayerDeliveryState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public static PlayerMovingState Init(PlayerStateMachine stateMachine)
    {
        return new PlayerMovingState(stateMachine,
            new PlayerDeliveryState(stateMachine),
            stateMachine.Player.GetWorkingClearCounter());
    }

    public static PlayerMovingState Deliver(PlayerStateMachine stateMachine)
    {
        return new PlayerMovingState(stateMachine,
            new PlayerDeliveryState(stateMachine),
            stateMachine.GetNearestCounter<DeliveryCounter>());
    }

    public override void Enter()
    {
        if (!stateMachine.Player.HasKitchenObject() || !stateMachine.Player.GetKitchenObject().TryGetPlate(out var _))
        {
            Debug.Log($"Player {stateMachine.Player.name} does not have a PlateKitchenObject to deliver.");

            GameInput.Instance.Interact();

            stateMachine.ChangeState(Deliver(stateMachine));
        }
        else
        {
            Debug.Log($"Player {stateMachine.Player.name} is delivering the plate.");

            GameInput.Instance.Interact();

            DeliveryManager.Instance.OnRecipeDelivered += DeliveryManager_OnRecipeDelivered;
        }
    }

    public override void Exit()
    {
        DeliveryManager.Instance.OnRecipeDelivered -= DeliveryManager_OnRecipeDelivered;
    }

    private void DeliveryManager_OnRecipeDelivered()
    {
        DeliveryManager.Instance.OnRecipeDelivered -= DeliveryManager_OnRecipeDelivered;

        stateMachine.SetCurrentRecipe(null);
        stateMachine.ChangeState(new PlayerIdleState(stateMachine));
    }
}
