using System.Linq;
using UnityEngine;

public class PlayerRecipePlanState : BaseState<PlayerStateMachine>
{
    public PlayerRecipePlanState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log($"Player {stateMachine.Player.name} entered Recipe Plan State");

        if (stateMachine.CurrentRecipe == null)
        {
            stateMachine.ChangeState(new PlayerIdleState(stateMachine));

            return;
        }

        if (stateMachine.PlateKitchenObject == null)
        {
            stateMachine.ChangeState(PlayerGetPlateState.Create(stateMachine));

            return;
        }

        var neededIngredients = stateMachine.CurrentRecipe.kitchenObjectSOs.Where(x => !stateMachine.PlateKitchenObject.GetKitchenObjectSOs().Contains(x));

        if (!neededIngredients.Any())
        {
            stateMachine.ChangeState(PlayerDeliveryState.Init(stateMachine));

            return;
        }

        if (stateMachine.IngredientIsReadyOnStove)
        {
            stateMachine.ChangeState(PlayerGetFromStoveState.Create(stateMachine));

            return;
        }

        foreach (var neededIngredient in neededIngredients.OrderByDescending(x => stateMachine.Player.TryGetFryingRecipe(x, out var _)))
        {
            if (stateMachine.Player.TryGetCuttingRecipe(neededIngredient, out var cuttingRecipeSO))
            {
                var nextState = PlayerGetIngredientState.Create(stateMachine, PlayerCuttingState.Create(stateMachine), cuttingRecipeSO.input);
                stateMachine.ChangeState(nextState);

                break;
            }
            else if (stateMachine.Player.TryGetFryingRecipe(neededIngredient, out var fryingRecipeSO))
            {
                if (stateMachine.StoveCounter != null)
                {
                    continue;
                }

                var nextState = PlayerGetIngredientState.Create(stateMachine, PlayerPutOnStoveState.Create(stateMachine), fryingRecipeSO.input);
                stateMachine.ChangeState(nextState);

                break;
            }
            else
            {
                var nextState = PlayerGetIngredientState.Create(stateMachine, PlayerPlaceOnPlateState.Create(stateMachine), neededIngredient);
                stateMachine.ChangeState(nextState);

                break;
            }
        }
    }

    public override void Update()
    {
        if (stateMachine.IngredientIsReadyOnStove)
        {
            stateMachine.ChangeState(PlayerGetFromStoveState.Create(stateMachine));
        }
    }
}
