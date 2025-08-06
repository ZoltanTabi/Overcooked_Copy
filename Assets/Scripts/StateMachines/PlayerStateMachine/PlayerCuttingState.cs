using UnityEngine;

public class PlayerCuttingState : BaseState<PlayerStateMachine>
{
    private CuttingCounter cuttingCounter;

    public PlayerCuttingState(PlayerStateMachine stateMachine) : base(stateMachine) { }

    public static PlayerMovingState Create(PlayerStateMachine stateMachine)
    {
        return new PlayerMovingState(
            stateMachine,
            new PlayerCuttingState(stateMachine),
            stateMachine.GetNearestCounter<CuttingCounter>());
    }

    public override void Enter()
    {
        Debug.Log($"Player {stateMachine.Player.name} entered Cutting State");

        GameInput.Instance.Interact();

        cuttingCounter = stateMachine.Player.GetSelectedCounter() as CuttingCounter;
        cuttingCounter.OnProgressChanged += CuttingCounter_OnProgressChanged;

        GameInput.Instance.InteractAlternateStart();
    }

    public override void Exit()
    {
        cuttingCounter.OnProgressChanged -= CuttingCounter_OnProgressChanged;
    }

    private void CuttingCounter_OnProgressChanged(float progress)
    {
        if (progress < 1f)
        {
            return;
        }

        GameInput.Instance.InteractAlternateEnd();

        GameInput.Instance.Interact();

        stateMachine.ChangeState(PlayerPlaceOnPlateState.Create(stateMachine));
    }
}
