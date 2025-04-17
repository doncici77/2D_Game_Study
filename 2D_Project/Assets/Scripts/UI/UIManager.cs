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
        coinText.text = "���� : " + GameManager.instance.coinCount;
        hpText.text = "ü�� : " + PlayerStats.Instance.currentHp;

        if(PlayerStats.Instance.skillType == SkillType.Dash)
        {
            skill.text = "��ų : �뽬";
        }
        else if(PlayerStats.Instance.skillType == SkillType.Grappling)
        {
            skill.text = "��ų : �׷��ø�";
        }

        if (PlayerStats.Instance.weaponType == WeaponType.Short)
        {
            weapon.text = "���� : �ٰŸ�";
        }
        else if (PlayerStats.Instance.weaponType == WeaponType.Long)
        {
            weapon.text = "���� : ���Ÿ�";
        }
    }
}
