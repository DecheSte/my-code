using UnityEngine;

public class PlayerCounterState : PlayerState
{
    public PlayerCounterState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        stateTimer = player.counterAttackDuration;
        player.anim.SetBool("SuccessCounter", false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.ZeroVelocity();

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && hit.GetComponent<Enemy>().CanBeStunned())
            {
                stateTimer = 2;
                player.anim.SetBool("SuccessCounter", true);
            }
        }

        if (stateTimer < 0 || triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
