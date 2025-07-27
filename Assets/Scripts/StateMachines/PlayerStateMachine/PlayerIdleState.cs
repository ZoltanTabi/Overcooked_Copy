using System.Linq;
using UnityEngine;

public class PlayerIdleState : BaseState<PlayerStateMachine>
{
    public PlayerIdleState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public override void Enter()
    {
        Debug.Log($"Player {stateMachine.Player.name} entered Idle State");
        GameInput.Instance.Move(Vector2.zero);

        DeliveryManager.Instance.OnRecipeSpawned += DeliveryManager_OnRecipeSpawned;
        DeliveryManager_OnRecipeSpawned();
    }

    public override void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log($"Player {stateMachine.Player.name} pressed H key to start the state machine");
            stateMachine.Start();
        }

        if (stateMachine.IsStarted && stateMachine.CurrentRecipe != null)
        {
            stateMachine.ChangeState(new PlayerRecipePlanState(stateMachine));
        }
    }

    public override void Exit()
    {
        DeliveryManager.Instance.OnRecipeSpawned -= DeliveryManager_OnRecipeSpawned;
    }

    private void DeliveryManager_OnRecipeSpawned()
    {
        var recipeSO = DeliveryManager.Instance.GetWaitingRecipeSOs().FirstOrDefault();

        if (recipeSO != null && stateMachine.CurrentRecipe == null)
        {
            stateMachine.SetCurrentRecipe(recipeSO);
        }
    }
}
