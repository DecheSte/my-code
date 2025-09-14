using UnityEngine;

public class CloneSkill : Skill
{
    [Header("Clone info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuration;

    [SerializeField] private bool canAttack;

    public void CreateClone(Transform _cloneposition, Vector3 _offset)
    {
        GameObject newClone = Instantiate(clonePrefab);

        newClone.GetComponent<CloneSkillController>().SetUpClone(_cloneposition, cloneDuration, canAttack, _offset);
    }
}
