using UnityEngine;

public class EnemyState
{
    protected EnemyStateMachine stateMachine;
    protected Enemy enemy;
    protected Rigidbody2D rb;
    protected bool triggerCalled;
    protected float stateTimer;
    private string animBoolName;

    public EnemyState(Enemy _enemy, EnemyStateMachine _statemachine, string _animBoolName)
    {
        this.enemy = _enemy;
        this.stateMachine = _statemachine;
        this.animBoolName = _animBoolName;
    }

    public virtual void Enter()
    {
        triggerCalled = false;
        enemy.anim.SetBool(animBoolName, true);
        rb = enemy.rb;
    }

    public virtual void Exit()
    {
        enemy.anim.SetBool(animBoolName, false);
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
    }

    public virtual void AnimationFinished() => triggerCalled = true;
}