using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Transform player;
    private EnemySkeleton enemyS;
    private int moveDir;
    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _statemachine, string _animBoolName, EnemySkeleton _enemy) : base(_enemyBase, _statemachine, _animBoolName)
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

        if (Vector2.Distance(player.position, enemyS.transform.position) < enemyS.attackDist)
        {
            enemyS.ZeroVelocity();
        }

        if (enemyS.IsPlayerDetected())
        {
            stateTimer = enemyS.battleTime;
            if (enemyS.IsPlayerDetected().distance < enemyS.attackDist && Time.time - enemyS.lastTimeAttack > enemyS.attackCoolDown)
                stateMachine.ChangeState(enemyS.attackState);
        }
        else
        {
            if (stateTimer < 0 && !enemyS.HasSword())
                stateMachine.ChangeState(enemyS.idleState);
        }


        if (player.position.x > enemyS.transform.position.x)
            moveDir = 1;
        else if (player.position.x < enemyS.transform.position.x)
            moveDir = -1;

        if (enemyS.IsGroundDetected() && Vector2.Distance(player.position, enemyS.transform.position) > enemyS.attackDist)
            enemyS.SetVelocity(enemyS.moveSpeed * 1.5f * moveDir, rb.linearVelocity.y);

    }
}