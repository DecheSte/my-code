using UnityEngine;

public class SkeletonStunState : EnemyState
{
    private EnemySkeleton enemyS;
    public SkeletonStunState(Enemy _enemyBase, EnemyStateMachine _statemachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _statemachine, _animBoolName)
    {
        this.enemyS = _enemy;
    }

    public override void Enter()
    {
        base.Enter();

        enemyS.fx.InvokeRepeating("RedColorBlink", 0, .1f);

        stateTimer = enemyS.stunDuration;

        enemyS.rb.linearVelocity = new Vector2(-enemyS.faceDir * enemyS.stunDir.x, enemyS.stunDir.y);
    }

    public override void Exit()
    {
        base.Exit();

        enemyS.fx.Invoke("CancelRedBlink", 0);
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            stateMachine.ChangeState(enemyS.idleState);
    }
}
