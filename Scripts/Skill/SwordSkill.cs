using System;
using UnityEngine;

public enum SwordType
{
    Regular,
    Bounce,
    Pierce,
    Spin
}

public class SwordSkill : Skill
{
    public SwordType swordType = SwordType.Regular;
    
    [Header("Bounce info")]
    [SerializeField] private int amountOfBounce;
    [SerializeField] private float bounceGravity = 1;
    [SerializeField] private float bounceSpeed;

    [Header("Pierce info")]
    [SerializeField] private int amountOfPierce;
    [SerializeField] private float pierceGravity = .5f;

    [Header("Spin info")]
    [SerializeField] private float hitCoolDown = .35f;
    [SerializeField] private float maxTravelDist = 7f;
    [SerializeField] private float spinDuration = 2f;
    [SerializeField] private float spinGravity = .5f;
    
    [Header("Sword info")]
    [SerializeField] private GameObject swordPrefab;
    [SerializeField] private Vector2 launchDir;
    [SerializeField] private float freezeTimeDuration;
    [SerializeField] private float returnSpeed;
    private float swordGravity;

    private Vector2 finalDir;

    [Header("Aim dots")]
    [SerializeField] private int dotsNum;
    [SerializeField] private float spaceBetweenDots;
    [SerializeField] private GameObject dotPrefab;
    [SerializeField] private Transform dotsParent;

    private GameObject[] dots;


    protected override void Start()
    {
        base.Start();
        GenerateDots();
    }

    private void SetUpGravity()
    {
        swordGravity = 4;
        if (swordType == SwordType.Bounce)
            swordGravity = bounceGravity;
        else if (swordType == SwordType.Pierce)
            swordGravity = pierceGravity;
        else if (swordType == SwordType.Spin)
            swordGravity = spinGravity;
    }

    protected override void Update()
    {
        SetUpGravity();
        if (Input.GetKeyUp(KeyCode.Mouse1))
            finalDir = new Vector2(AimPosition().normalized.x * launchDir.x, AimPosition().normalized.y * launchDir.y);

        if (Input.GetKey(KeyCode.Mouse1))
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.position = DotsPosition(i * spaceBetweenDots);
            }
        }
    }

    public void CreateSword()
    {
        GameObject newSword = Instantiate(swordPrefab, player.transform.position, transform.rotation);
        SwordSkillController newSwordScript = newSword.GetComponent<SwordSkillController>();


        switch (swordType)
        {
            case SwordType.Bounce:
                newSwordScript.SetUpBounce(true, amountOfBounce, bounceSpeed);
                break;
            case SwordType.Pierce:
                newSwordScript.SetUpPierce(amountOfPierce);
                break;
            case SwordType.Spin:
                newSwordScript.SetUpSpin(true, maxTravelDist, spinDuration, hitCoolDown);
                break;
        }


        newSwordScript.SetUpSword(finalDir, swordGravity, player, freezeTimeDuration, returnSpeed);

        player.AssignNewSword(newSword);

        DotsActive(false);
    }

    public Vector2 AimPosition()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - playerPosition;

        return direction;
    }

    #region AimInfo
    public void DotsActive(bool _isActive)
    {
        for(int i = 0; i < dots.Length; i++)
        {
            dots[i].SetActive(_isActive);
        }
    }
    
    private void GenerateDots()
    {
        dots = new GameObject[dotsNum];
        for(int i = 0; i < dotsNum; i++)
        {
            dots[i] = Instantiate(dotPrefab, player.transform.position, Quaternion.identity, dotsParent);
            dots[i].SetActive(false);
        }
    }

    private Vector2 DotsPosition(float t)
    {
        Vector2 position = (Vector2)player.transform.position + new Vector2(
            AimPosition().normalized.x * launchDir.x,
            AimPosition().normalized.y * launchDir.y) * t + .5f * (Physics2D.gravity * swordGravity) * (t * t);

        return position;
    }
    #endregion
}
