using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text coinText;
    public Slider hpSlider;
    public GameObject dash;
    public GameObject grappling;
    public GameObject wShort;
    public GameObject wLong;
    public GameObject dead;

    void Update()
    {
        if (GameManager.instance != null)
        {
            coinText.text = "ÄÚÀÎ : " + GameManager.instance.coinCount;
        }
        hpSlider.value = (float)PlayerStats.Instance.currentHp / (float)PlayerStats.Instance.maxHp;

        if (PlayerStats.Instance.skillType == SkillType.Dash)
        {
            dash.SetActive(true);
            grappling.SetActive(false);
        }
        else if(PlayerStats.Instance.skillType == SkillType.Grappling)
        {
            dash.SetActive(false);
            grappling.SetActive(true);
        }

        if (PlayerStats.Instance.weaponType == WeaponType.Short)
        {
            wShort.SetActive(true);
            wLong.SetActive(false);
        }
        else if (PlayerStats.Instance.weaponType == WeaponType.Long)
        {
            wShort.SetActive(false);
            wLong.SetActive(true);
        }
    }

    public void Dead(bool set)
    {
        dead.SetActive(set);
    }
}
