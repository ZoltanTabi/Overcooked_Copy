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
            stateMachine.ChangeState(new PlayerGetPlateState(stateMachine));

            return;
        }

        var neededIngredients = stateMachine.CurrentRecipe.kitchenObjectSOs.Where(x => !stateMachine.PlateKitchenObject.GetKitchenObjectSOs().Contains(x));

        if (!neededIngredients.Any())
        {
            stateMachine.ChangeState(new PlayerDeliveryState(stateMachine));

            return;
        }

        var neededIngredient = neededIngredients.FirstOrDefault();

        if (stateMachine.Player.TryGetCuttingRecipeForOutput(neededIngredient, out var cuttingRecipeSO))
        {
            stateMachine.ChangeState(new PlayerCuttingState(stateMachine, cuttingRecipeSO));
        }
        else if (stateMachine.Player.TryGetFryingRecipeForOutput(neededIngredient, out var fryingRecipeSO))
        {
            stateMachine.ChangeState(new PlayerFryingState(stateMachine, fryingRecipeSO));
        }
        else
        {
            stateMachine.ChangeState(new PlayerGetIngredientState(stateMachine, neededIngredient));
        }
    }
}
