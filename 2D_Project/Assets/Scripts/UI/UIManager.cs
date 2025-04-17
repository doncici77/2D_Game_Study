using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text coinText;
    public Slider hpSlider;
    public Text skill;
    public Text weapon;
    public GameObject dead;

    void Start()
    {
        
    }

    void Update()
    {
        coinText.text = "코인 : " + GameManager.instance.coinCount;
        hpSlider.value = (float)PlayerStats.Instance.currentHp / (float)PlayerStats.Instance.maxHp;

        if (PlayerStats.Instance.skillType == SkillType.Dash)
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

    public void Dead(bool set)
    {
        dead.SetActive(set);
    }
}
