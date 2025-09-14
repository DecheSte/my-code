using UnityEngine;

public class BlackholeSkill : Skill
{
    [SerializeField] private GameObject blackholePrefab;
    [SerializeField] private float maxSize;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [Space]
    [SerializeField] private int amountOfAttack;
    [SerializeField] private float cloneAttackCoolDown;
    [SerializeField] private float blackholeDuration;

    public BlackholeSkillController currBlackhole;
    public override bool CanUseSkill()
    {
        return base.CanUseSkill();
    }

    public override void UseSkill()
    {
        base.UseSkill();

        GameObject newBlackhole = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);

        currBlackhole = newBlackhole.GetComponent<BlackholeSkillController>();
        currBlackhole.SetUpBlackhole(maxSize, growSpeed, shrinkSpeed, amountOfAttack, cloneAttackCoolDown, blackholeDuration);
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool BlackholeFinished()
    {
        if (!currBlackhole)
            return false;

        if (currBlackhole.playerCanExitState)
        {
            currBlackhole = null;
            return true;
        }
        return false;
    }
}
