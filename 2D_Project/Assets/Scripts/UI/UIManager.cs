using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text coinText;
    public Text hpText;

    void Start()
    {
        
    }

    void Update()
    {
        coinText.text = "���� : " + GameManager.instance.coinCount;
        hpText.text = "ü�� : " + PlayerStats.Instance.currentHp;
    }
}
