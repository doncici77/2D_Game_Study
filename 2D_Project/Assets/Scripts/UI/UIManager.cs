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
        coinText.text = "코인 : " + GameManager.instance.coinCount;
        hpText.text = "체력 : " + PlayerStats.Instance.currentHp;
    }
}
