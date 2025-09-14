using UnityEngine;

public class CloneSkillController : MonoBehaviour
{
    private SpriteRenderer sr;
    private Animator anim;

    [SerializeField] private float colorLosingSpeed;
    [SerializeField] private float cloneDuration;
    private float cloneTimer;

    [SerializeField] private Transform attackCheck;
    [SerializeField] private float attackRadius = .8f;
    private Transform closestEnemy;
    

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        cloneTimer -= Time.deltaTime;

        if (cloneTimer < 0)
        {
            sr.color = new Color(1, 1, 1, sr.color.a - (Time.deltaTime * colorLosingSpeed));

            if (sr.color.a <= 0)
                Destroy(gameObject);
        }
    }

    public void SetUpClone(Transform _newtransform, float _cloneduration, bool _canAttack, Vector3 _offset)
    {
        if (_canAttack)
            anim.SetInteger("AttackNum", Random.Range(1, 4));
        
        transform.position = _newtransform.position + _offset;
        cloneTimer = _cloneduration;

        FaceClosestEnemy();
    }

    public void AnimationTriggerCall()
    {
        cloneTimer = -.1f;
    }

    public void AttackTriggerCall()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(attackCheck.position, attackRadius);

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().Damage();
        }
    }

    private void FaceClosestEnemy()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDist = Mathf.Infinity;
        float distToEnemy;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                distToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                if (distToEnemy < closestDist)
                {
                    closestEnemy = hit.transform;
                    closestDist = distToEnemy;
                }
            }
        }
        if (closestEnemy != null)
        {
            if (transform.position.x > closestEnemy.position.x)
                transform.Rotate(0, 180, 0);
        }
    }
}
