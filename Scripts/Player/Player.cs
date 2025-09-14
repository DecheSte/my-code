using UnityEngine;
using System.Collections;

public class Player : Entity
{
    public SkillManager skill;
    public bool isBusy { get; private set; }

    [Header("Sword info")]
    public GameObject sword { get; private set; }
    public float catchForce;

    [Header("Move info")]
    public float jumpForce;
    public float slideSpeed;
    public float moveSpeed;

    [Header("Attack details")]
    public Vector2[] attackMovement;
    public float counterAttackDuration;
    public float counterCoolDown;

    [Header("Dash info")]
    public float dashForce;
    public float dashDuration;
    public float dashDir;


    #region States
    public PlayerStateMachine stateMachine { get; private set; }
    public PlayerIdleState idleState { get; private set; }
    public PlayerMoveState moveState { get; private set; }
    public PlayerAirState airState { get; private set; }
    public PlayerJumpState jumpState { get; private set; }
    public PlayerDashState dashState { get; private set; }
    public PlayerWallSlideState wallSlideState { get; private set; }
    public PlayerWallJumpState wallJumpState { get; private set; }
    public PlayerCounterState counterState { get; private set; }
    public PlayerAimSwordState aimState { get; private set; }
    public PlayerCatchSwordState catchState { get; private set; }
    public PlayerBlackholeState blackholeState { get; private set; }

    public PlayerPrimaryAttackState primaryAttack { get; private set; }
    #endregion

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new PlayerStateMachine();
        idleState = new PlayerIdleState(this, stateMachine, "Idle");
        moveState = new PlayerMoveState(this, stateMachine, "Move");
        jumpState = new PlayerJumpState(this, stateMachine, "Jump");
        airState  = new PlayerAirState(this, stateMachine, "Jump");
        dashState = new PlayerDashState(this, stateMachine, "Dash");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "Slide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
        primaryAttack = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
        counterState = new PlayerCounterState(this, stateMachine, "CounterAttack");
        aimState = new PlayerAimSwordState(this, stateMachine, "Aim");
        catchState = new PlayerCatchSwordState(this, stateMachine, "Catch");
        blackholeState = new PlayerBlackholeState(this, stateMachine, "Jump");
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        skill = SkillManager.instance;
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currState.Update();
        CheckDashInput();
    }

    public void AssignNewSword(GameObject _newSword)
    {
        sword = _newSword;
    }

    public void CatchSword()
    {
        stateMachine.ChangeState(catchState);
        Destroy(sword);
    }

    public void DestroySword()
    {
        Destroy(sword);
    }

    public IEnumerator BusyFor(float _seconds)
    {
        isBusy = true;

        yield return new WaitForSeconds(_seconds);

        isBusy = false;
    }


    private void CheckDashInput()
    {

        if (IsWallDetected())
            return;

        if (Input.GetKeyDown(KeyCode.LeftShift) && SkillManager.instance.dash.CanUseSkill() && !skill.blackhole.currBlackhole)
        {
            stateMachine.ChangeState(dashState);
            dashDir = Input.GetAxisRaw("Horizontal");
            if (dashDir == 0)
                dashDir = faceDir;
        }
    }

    public void AnimationFinished() => stateMachine.currState.AnimationTrigger();


}
