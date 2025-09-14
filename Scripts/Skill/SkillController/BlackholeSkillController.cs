using UnityEngine;
using System.Collections.Generic;

public class BlackholeSkillController : MonoBehaviour
{
    [SerializeField] private GameObject hotkeyPrefab;
    [SerializeField] private List<KeyCode> keyCodeList;
    
    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private bool canGrow = true;
    private bool canShrink;
    private bool canCreateHotkey = true;
    private bool cloneAttackReleased;
    private float blackholeTimer;

    private int amountOfAttack = 4;
    private float cloneAttackCoolDown = .3f;
    private float cloneAttackTimer;

    private List<Transform> targets = new List<Transform>();
    private List<GameObject> createdHotkey = new List<GameObject>();

    public bool playerCanExitState { get; private set; }
    public void SetUpBlackhole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttack, float _cloneAttackCoolDown, float _blackholeDuration)
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttack = _amountOfAttack;
        cloneAttackCoolDown = _cloneAttackCoolDown;
        blackholeTimer = _blackholeDuration;
    }
    
    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;

        if (blackholeTimer < 0) {
            blackholeTimer = Mathf.Infinity;
            if (targets.Count > 0)
                ReleaseCloneAttack();
            else
                FinishBlackholeAbility();
        }

        CloneAttackLogic();

        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x <= 0)
                Destroy(gameObject);
        }
    }

    private void ReleaseCloneAttack()
    {
        if (targets.Count <= 0)
            return;
        
        DestroyHotkeys();
        canCreateHotkey = false;
        cloneAttackReleased = true;
        PlayerManager.instance.player.MakeTransparent(true);
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer <= 0 && cloneAttackReleased && amountOfAttack > 0)
        {
            cloneAttackTimer = cloneAttackCoolDown;

            int randomIndex = Random.Range(0, targets.Count);
            float xoffset;
            xoffset = Random.Range(0, 100) > 50 ? 2 : -2;
            SkillManager.instance.clone.CreateClone(targets[randomIndex], new Vector3(xoffset, 0));
            amountOfAttack--;
            if (amountOfAttack <= 0)
            {
                Invoke("FinishBlackholeAbility", .5f);
            }
        }
    }

    private void FinishBlackholeAbility()
    {
        DestroyHotkeys();
        playerCanExitState = true;
        canShrink = true;
        cloneAttackReleased = false;
    }

    public void DestroyHotkeys()
    {
        if (createdHotkey.Count <= 0)
            return;

        for (int i = 0; i < createdHotkey.Count; i++)
        {
            Destroy(createdHotkey[i]);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {      
        if (collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            CreateHotkey(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) => collision.GetComponent<Enemy>()?.FreezeTime(false);

    private void CreateHotkey(Collider2D collision)
    {
        if (keyCodeList.Count <= 0)
            return;

        if (!canCreateHotkey)
            return;

        GameObject newHotkey = Instantiate(hotkeyPrefab, collision.transform.position + new Vector3(0, 2), Quaternion.identity);
        createdHotkey.Add(newHotkey);

        KeyCode chooseKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(chooseKey);

        BlackholeHotkeyController newHotkeyScript = newHotkey.GetComponent<BlackholeHotkeyController>();
        newHotkeyScript.SetupHotkey(chooseKey, collision.transform, this);
    }

    public void AddEnemyToList(Transform enemyTransform) => targets.Add(enemyTransform);
}
