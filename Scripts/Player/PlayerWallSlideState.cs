using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (Input.GetKeyDown(KeyCode.Space)) {
            stateMachine.ChangeState(player.wallJumpState);
            return;
        }

        if (xInput != 0 && xInput * player.faceDir < 0)
        {
            player.StartCoroutine("BusyFor", 1f);

            if (!player.isBusy)
                stateMachine.ChangeState(player.idleState);
        }

        if (yInput < 0)
            player.rb.linearVelocity = new Vector2(0, player.rb.linearVelocity.y);
        else
            player.rb.linearVelocity = new Vector2(0, player.rb.linearVelocity.y * player.slideSpeed);

        if (player.IsGroundDetected())
            stateMachine.ChangeState(player.idleState);
    }
}
