using UnityEngine;

public class PlayerAnimationTriggers : MonoBehaviour
{
    public Player player => GetComponentInParent<Player>();

    public void AnimationTriggerCall()
    {
        player.AnimationFinished();
    }

    public void AttackTriggerCall()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.attackCheck.position, player.attackRadius);

        foreach(var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
                hit.GetComponent<Enemy>().Damage();
        }
    }

    public void ThrowSwordTrigger()
    {
        SkillManager.instance.sword.CreateSword();
    }
}
