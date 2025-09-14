using UnityEngine;

public class SkeletonAttackState : EnemyState
{
    private EnemySkeleton enemyS;
    public SkeletonAttackState(Enemy _enemyBase, EnemyStateMachine _statemachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _statemachine, _animBoolName)
    {
        this.enemyS = _enemy;
    }

    public override void Enter()
    {
        base.Enter();


    }

    public override void Exit()
    {
        base.Exit();

        enemyS.lastTimeAttack = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (enemyS.IsGroundDetected())
            enemyS.ZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(enemyS.battleState);
    }
}
