using UnityEngine;
using System.Collections.Generic;

public class SwordSkillController : MonoBehaviour
{
    private Animator anim;
    private Rigidbody2D rb;
    private CircleCollider2D cd;
    private Player player;

    private bool canRotate = true;
    private bool isReturning;
    private float returnSpeed = 50;

    private float freezeTimeDuration;
    
    [Header("Pierce info")]
    private int amountOfPierce;

    [Header("Bounce info")]
    public bool isBouncing;
    public int amountOfBounce;
    private List<Transform> enemyTarget;
    private int targetIndex;
    [SerializeField] private float bounceSpeed;

    [Header("Spin info")]
    private float maxTravelDist;
    private float spinDuration;
    private float spinTimer;
    private bool isStopped;
    private bool isSpinning;
    private float hitTimer;
    private float hitCoolDown;
    private float spinDir;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cd = GetComponent<CircleCollider2D>();

    }

    public void SetUpBounce(bool _isBouncing, int _amountOfBounce, float _bounceSpeed)
    {
        isBouncing = _isBouncing;
        amountOfBounce = _amountOfBounce;

        enemyTarget = new List<Transform>();
        bounceSpeed = _bounceSpeed;
    }

    public void SetUpPierce(int _amountOfPierce)
    {
        amountOfPierce = _amountOfPierce;
    }

    public void SetUpSpin(bool _isSpinning, float _maxTravelDist, float _spinDuration, float _hitCoolDown)
    {
        isSpinning = _isSpinning;
        maxTravelDist = _maxTravelDist;
        spinDuration = _spinDuration;
        hitCoolDown = _hitCoolDown;
        spinTimer = spinDuration;
    }
    
    public void SetUpSword(Vector2 _dir, float _gravityScale, Player _player, float _freezeTimeDuration, float _returnSpeed)
    {
        rb.linearVelocity = _dir;
        rb.gravityScale = _gravityScale;
        player = _player;
        freezeTimeDuration = _freezeTimeDuration;
        returnSpeed = _returnSpeed;

        if (amountOfPierce <= 0)
            anim.SetBool("Rotation", true);

        spinDir = Mathf.Clamp(rb.linearVelocity.x, -1, 1);
    }

    public void ReturnSword()
    {
        rb.constraints = RigidbodyConstraints2D.FreezeAll;
        transform.parent = null;
        isReturning = true;
    }

    private void Update()
    {
        if (canRotate)
            transform.right = rb.linearVelocity;

        if (isReturning)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, returnSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, player.transform.position) < .5f)
            {
                player.CatchSword();
                isReturning = false;
            }
        }

        BounceLogic();

        SpinLogic();
    }

    private void SpinLogic()
    {
        if (isSpinning)
        {
            if (Vector2.Distance(player.transform.position, transform.position) >= maxTravelDist && !isStopped)
            {
                Debug.Log("Multiple called");
                StopWhenSpinning();
            }

            if (isStopped)
            {
                spinTimer -= Time.deltaTime;


                if (spinTimer <= 0)
                {
                    isReturning = true;
                    isSpinning = false;
                }

                if (spinTimer > 0)
                {
                    transform.position = Vector2.MoveTowards(transform.position, new Vector2(transform.position.x + spinDir, transform.position.y), 1.5f * Time.deltaTime);
                }

                hitTimer -= Time.deltaTime;

                if (hitTimer <= 0)
                {
                    hitTimer = hitCoolDown;

                    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 1);

                    foreach (var hit in colliders)
                    {
                        if (hit.GetComponent<Enemy>() != null)
                            SwordSkillDamage(hit.GetComponent<Enemy>());
                    }
                }
            }
        }
    }

    private void StopWhenSpinning()
    {
        isStopped = true;
        rb.constraints = RigidbodyConstraints2D.FreezePosition;
    }

    private void BounceLogic()
    {
        if (isBouncing && enemyTarget.Count > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, enemyTarget[targetIndex].position, bounceSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, enemyTarget[targetIndex].position) < .1f)
            {
                SwordSkillDamage(enemyTarget[targetIndex++].GetComponent<Enemy>());                
                
                amountOfBounce--;

                if (amountOfBounce <= 0)
                {
                    isBouncing = false;
                    isReturning = true;
                }

                if (targetIndex == enemyTarget.Count)
                    targetIndex = 0;
            }
        }
        if (Vector2.Distance(transform.position, player.transform.position) > 50)
            player.DestroySword();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isReturning)
            return;

        if (collision.GetComponent<Enemy>() != null)
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            SwordSkillDamage(enemy);
        }

        collision.GetComponent<Enemy>()?.Damage();

        SetUpTargetForBounce(collision);

        StuckInto(collision);
    }

    private void SwordSkillDamage(Enemy enemy)
    {
        enemy.Damage();
        enemy.StartCoroutine("FreezeTimerFor", freezeTimeDuration);
    }

    private void SetUpTargetForBounce(Collider2D collision)
    {
        if (collision.GetComponent<Enemy>() != null)
        {
            if (isBouncing && enemyTarget.Count <= 0)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 10);

                foreach (var hit in colliders)
                {
                    if (hit.GetComponent<Enemy>() != null)
                        enemyTarget.Add(hit.transform);
                }
            }
        }
    }

    private void StuckInto(Collider2D collision)
    {
        if (isSpinning)
        {
            StopWhenSpinning();
            return;
        }
        
        if (amountOfPierce > 0 && collision.GetComponent<Enemy>() != null)
        {
            amountOfPierce--;
            return;
        }

        canRotate = false;
        cd.enabled = false;

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        if (isBouncing && enemyTarget.Count > 0)
            return;

        anim.SetBool("Rotation", false);
        transform.parent = collision.transform;
    }
}
