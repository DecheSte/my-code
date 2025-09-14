using UnityEngine;
using System.Collections;

public class Entity : MonoBehaviour
{
    
    [Header("Collision info")]
    public Transform attackCheck;
    public float attackRadius;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDist;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDist;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected LayerMask whatIsWall;


    #region Components
    public Animator anim { get; protected set; }
    public Rigidbody2D rb { get; protected set; }
    public EntityFX fx { get; private set; }
    public SpriteRenderer sr { get; private set; }
    #endregion


    public int faceDir { get; protected set; } = 1;
    protected bool faceRight = true;

    [Header("Knock info")]
    [SerializeField] protected Vector2 knockBackDir;
    [SerializeField] protected float knockBackDuration;
    public bool isKnocked;

    protected virtual void Awake()
    {
        fx = GetComponent<EntityFX>();
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponentInChildren<SpriteRenderer>();
    }

    protected virtual void Start()
    {
        
    }

    protected virtual void Update()
    {
        
    }

    public void Damage()
    {
        fx.StartCoroutine("FlashFX");
        StartCoroutine("HitKnockBack");
    }

    protected IEnumerator HitKnockBack()
    {
        isKnocked = true;
        rb.linearVelocity = new Vector2(knockBackDir.x * -faceDir, knockBackDir.y);
        yield return new WaitForSeconds(knockBackDuration);
        isKnocked = false;
    }

    #region Collision
    public virtual bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDist, whatIsGround);
    public virtual bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right * faceDir, wallCheckDist, whatIsGround);

    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDist));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDist, wallCheck.position.y));
        Gizmos.DrawSphere(transform.position, 0.1f);
        Gizmos.DrawWireSphere(attackCheck.position, attackRadius);
    }
    #endregion

    #region Flip
    public void Flip()
    {
        faceDir *= -1;
        faceRight = !faceRight;
        transform.Rotate(0, 180, 0);
    }

    public void FlipController(float _x)
    {
        if (_x > 0 && !faceRight)
            Flip();
        else if (_x < 0 && faceRight)
            Flip();
    }
    #endregion

    #region Velocity
    public void ZeroVelocity() => SetVelocity(0, 0);
    public void SetVelocity(float _xV, float _yV)
    {
        if (isKnocked)
            return;

        rb.linearVelocity = new Vector2(_xV, _yV);
        FlipController(_xV);
    }
    #endregion

    public void MakeTransparent(bool _transparent)
    {
        if (_transparent)
            sr.color = Color.clear;
        else
            sr.color = Color.white;
    }
}