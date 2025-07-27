using UnityEngine;

public class PlayerMovingState : BaseState<PlayerStateMachine>
{
    private readonly BaseState<PlayerStateMachine> nextState = null;
    private readonly BaseCounter targetCounter = null;
    private Vector2 moveDirection = Vector2.zero;

    public PlayerMovingState(PlayerStateMachine stateMachine, BaseState<PlayerStateMachine> nextState, BaseCounter targetCounter) : base(stateMachine)
    {
        this.nextState = nextState;
        this.targetCounter = targetCounter;
    }

    public override void Enter()
    {
        Debug.Log($"Player {stateMachine.Player.name} entered Moving State to {targetCounter.name} and next state {nextState?.GetType().Name}");

        stateMachine.Player.OnCounterSelected += Player_OnCounterSelected;

        moveDirection = GetMoveDirection();

        Player_OnCounterSelected(stateMachine.Player.GetSelectedCounter());
    }

    public override void Update()
    {
        if (moveDirection != Vector2.zero)
        {
            moveDirection = GetMoveDirection();
        }

        GameInput.Instance.Move(moveDirection);
        //GameInput.Instance.Dash();
    }

    public override void Exit()
    {
        stateMachine.Player.OnCounterSelected -= Player_OnCounterSelected;
        GameInput.Instance.Move(Vector2.zero);
    }

    private void Player_OnCounterSelected(BaseCounter selectedCounter)
    {
        if (selectedCounter == targetCounter)
        {
            moveDirection = Vector2.zero;
            stateMachine.ChangeState(nextState);
        }
    }

    public Vector2 GetMoveDirection()
    {
        if (targetCounter == null)
        {
            return Vector2.zero;
        }

        Vector3 dir3D = targetCounter.transform.position - stateMachine.Player.transform.position;

        return new Vector2(dir3D.x, dir3D.z).normalized;
    }
}
