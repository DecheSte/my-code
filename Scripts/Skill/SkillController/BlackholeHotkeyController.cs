using UnityEngine;
using TMPro;

public class BlackholeHotkeyController : MonoBehaviour
{
    private KeyCode myHotkey;
    private TextMeshProUGUI myText;
    private Transform myEnemy;
    private BlackholeSkillController myBlackhole;
    private SpriteRenderer sr;

    public void SetupHotkey(KeyCode _myHotkey, Transform _myEnemy, BlackholeSkillController _myBlackhole)
    {
        sr = GetComponent<SpriteRenderer>();
        myText = GetComponentInChildren<TextMeshProUGUI>();
        myHotkey = _myHotkey;
        myEnemy = _myEnemy;
        myBlackhole = _myBlackhole;
        myText.text = myHotkey.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(myHotkey))
        {
            myBlackhole.AddEnemyToList(myEnemy);

            myText.color = Color.clear;
            sr.color = Color.clear;
        }
    }
}
