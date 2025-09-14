using UnityEngine;

public class SkeletonMoveState : SkeletonGroundState
{
    public SkeletonMoveState(Enemy _enemyBase, EnemyStateMachine _statemachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _statemachine, _animBoolName, _enemy)
    {
    }

    public override void Enter()
    {
        base.Enter();
        enemy.SetVelocity(0, 0);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        enemyS.SetVelocity(enemyS.moveSpeed * enemyS.faceDir, rb.linearVelocity.y);
        if (enemyS.IsWallDetected() || !enemyS.IsGroundDetected())
        {
            enemyS.Flip();
            stateMachine.ChangeState(enemyS.idleState);
        }

    }
}
