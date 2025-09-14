using UnityEngine;
using System.Collections;

public class Enemy : Entity
{
    public EnemyStateMachine stateMachine { get; private set; }

    [Header("Stun info")]
    public float stunDuration;
    public Vector2 stunDir;
    protected bool canBeStunned;
    [SerializeField] protected GameObject counterImage;

    [SerializeField] protected LayerMask whatIsPlayer;

    [Header("Move info")]
    public float idleTime;
    public float moveSpeed;
    protected float defaultMoveSpeed = 1.5f;

    [Header("Attack info")]
    public float attackDist;
    public float attackCoolDown;
    [HideInInspector] public float lastTimeAttack;
    public float battleTime;

    public float enemySight;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
        stateMachine.currState.Update();
    }

    public virtual void FreezeTime(bool _timeFrozen)
    {
        if (_timeFrozen)
        {
            moveSpeed = 0;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultMoveSpeed;
            anim.speed = 1;
        }
    }

    protected virtual IEnumerator FreezeTimerFor(float _seconds)
    {
        FreezeTime(true);

        yield return new WaitForSeconds(_seconds);

        FreezeTime(false);
    }

    #region Counter Attack Window
    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }
    #endregion

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + attackDist * faceDir, transform.position.y));
        Debug.DrawRay(wallCheck.position, Vector2.right * faceDir * 50, Color.red);
    }

    public virtual RaycastHit2D IsPlayerDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * faceDir, 15, whatIsPlayer);

    public virtual void AnimationFinishedTrigger() => stateMachine.currState.AnimationFinished();
}
