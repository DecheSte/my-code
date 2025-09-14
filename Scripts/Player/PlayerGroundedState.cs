using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    private float counterTimer;
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName){}

    public override void Enter()
    {
        base.Enter();
        counterTimer = player.counterCoolDown;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        counterTimer -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.R))
            stateMachine.ChangeState(player.blackholeState);
        if (Input.GetKeyDown(KeyCode.Mouse1) && HasNoSword())
            stateMachine.ChangeState(player.aimState);
        
        if (Input.GetKeyDown(KeyCode.E) && counterTimer < 0)
        {
            counterTimer = player.counterCoolDown;
            stateMachine.ChangeState(player.counterState);
        }

        if (Input.GetKeyDown(KeyCode.Mouse0) && !player.sword)
            stateMachine.ChangeState(player.primaryAttack);

        if (!player.IsGroundDetected())
            stateMachine.ChangeState(player.airState);

        if (Input.GetKeyDown(KeyCode.Space) && player.IsGroundDetected())
            stateMachine.ChangeState(player.jumpState);
    }

    private bool HasNoSword() {
        if (!player.sword)
            return true;

        player.sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;
    }
}
