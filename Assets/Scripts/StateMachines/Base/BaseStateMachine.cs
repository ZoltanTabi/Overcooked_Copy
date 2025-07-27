
public abstract class BaseStateMachine<T> where T : BaseStateMachine<T>
{
    public BaseState<T> CurrentState { get; private set; }

    public void Initialize(BaseState<T> startingState)
    {
        CurrentState = startingState;
        CurrentState.Enter();
    }

    public void ChangeState(BaseState<T> newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    public virtual void Update()
    {
        CurrentState?.Update();
    }
}
