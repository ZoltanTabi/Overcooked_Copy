
public abstract class BaseState<T> where T : BaseStateMachine<T>
{
    protected T stateMachine;

    protected BaseState(T stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public virtual void Enter() { }

    public virtual void Update() { }

    public virtual void Exit() { }
}
