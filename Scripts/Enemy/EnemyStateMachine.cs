using UnityEngine;

public class EnemyStateMachine
{
    public EnemyState currState { get; private set; }

    public void Initialize(EnemyState _startState)
    {
        currState = _startState;
        currState.Enter();
    }

    public void ChangeState(EnemyState _newState)
    {
        currState.Exit();
        currState = _newState;
        currState.Enter();
    }
}
