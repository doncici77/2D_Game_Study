using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text coinText;
    public Text hpText;
    public Text skill;
    public Text weapon;

    void Start()
    {
        
    }

    void Update()
    {
        coinText.text = "코인 : " + GameManager.instance.coinCount;
        hpText.text = "체력 : " + PlayerStats.Instance.currentHp;

        if(PlayerStats.Instance.skillType == SkillType.Dash)
        {
            skill.text = "스킬 : 대쉬";
        }
        else if(PlayerStats.Instance.skillType == SkillType.Grappling)
        {
            skill.text = "스킬 : 그래플링";
        }

        if (PlayerStats.Instance.weaponType == WeaponType.Short)
        {
            weapon.text = "무기 : 근거리";
        }
        else if (PlayerStats.Instance.weaponType == WeaponType.Long)
        {
            weapon.text = "무기 : 원거리";
        }
    }
}
