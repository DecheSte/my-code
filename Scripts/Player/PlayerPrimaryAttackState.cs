using UnityEngine;

public class PlayerPrimaryAttackState : PlayerState
{
    private int comboCounter;
    private float lastTimeAttack;
    private float comboWindow = .7f;
    public PlayerPrimaryAttackState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName){}

    public override void Enter()
    {
        base.Enter();

        xInput = 0;
        
        if (comboCounter > 2 || Time.time > lastTimeAttack + comboWindow)
            comboCounter = 0;

        player.anim.SetInteger("comboCounter", comboCounter);

        stateTimer = .1f;

        float attackDir = player.faceDir;

        if (xInput != 0)
            attackDir = xInput;

        player.SetVelocity(player.attackMovement[comboCounter].x * attackDir, player.attackMovement[comboCounter].y);
    }

    public override void Exit()
    {
        base.Exit();

        player.StartCoroutine("BusyFor", .5f);

        comboCounter++;
        lastTimeAttack = Time.time;
    }

    public override void Update()
    {
        base.Update();

        if (stateTimer < 0)
            player.ZeroVelocity();

        if (triggerCalled)
            stateMachine.ChangeState(player.idleState);
    }
}
