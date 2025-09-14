using UnityEngine;

public class SkeletonGroundState : EnemyState
{
    protected EnemySkeleton enemyS;

    protected Transform player;
    public SkeletonGroundState(Enemy _enemyBase, EnemyStateMachine _statemachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _statemachine, _animBoolName)
    {
        this.enemyS = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        if (enemyS.HasSword() || enemyS.IsPlayerDetected() || Vector2.Distance(enemyS.transform.position, player.position) < 2)
            stateMachine.ChangeState(enemyS.battleState);
    }

}