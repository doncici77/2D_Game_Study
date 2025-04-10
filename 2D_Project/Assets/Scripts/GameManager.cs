using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int coinCount = 0;

    public Text coinText;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCoin(int amount)
    {
        coinCount += amount;
        coinText.text = "코인 : " + coinCount.ToString();
        SoundManager.Instance.PlaySFX(SFXType.ItemGet);
        PlayerPrefs.SetInt("Coin", coinCount);
    }

    public int GetCoinCount()
    {
        return coinCount;
    }

    public void ResetCoin()
    {
        coinCount = 0;
        coinText.text = "코인 : " + coinCount.ToString();
        PlayerPrefs.SetInt("Coin", coinCount);
    }
}
