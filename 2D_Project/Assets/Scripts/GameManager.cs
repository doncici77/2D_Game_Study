using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private int coinCount = 0;

    public Text coinText;

    private const string COIN_KEY = "CoinCount";
    private const string DAMAGE_KEY = "PlayerDamage";
    private const string ATTACK_SPEED_KEY = "PlayerAttackSpeed";
    private const string MOVE_SPEED_KEY = "PlayerMoveSpeed";
    private const string HP_KEY = "PlayerHp";

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
        coinText.text = "内牢 : " + coinCount.ToString();
        SaveCoin();
        SoundManager.Instance.PlaySFX(SFXType.ItemGet);
    }

    public bool UseCoin(int amount)
    {
        if(coinCount >= amount)
        {
            coinCount -= amount;
            SaveCoin();
            return true;
        }

        Debug.Log("内牢 何练");
        return false;
    }

    public int GetCoinCount()
    {
        return coinCount;
    }

    private void SaveCoin()
    {
        PlayerPrefs.SetInt(COIN_KEY, coinCount);
        PlayerPrefs.Save();
    }

    private void LoadCoin()
    {
        coinCount = PlayerPrefs.GetInt(COIN_KEY, 0);
    }

    public void SavePlayerStats(PlayerStats stats)
    {
        PlayerPrefs.SetInt(DAMAGE_KEY, stats.damage);
        PlayerPrefs.SetFloat(MOVE_SPEED_KEY, stats.moveSpeed);
        PlayerPrefs.SetFloat(ATTACK_SPEED_KEY, stats.attackSpeed);
        PlayerPrefs.SetInt(HP_KEY, stats.maxHp);
    }

    public void LoadPlayerStats(PlayerStats stats)
    {
        if(PlayerPrefs.HasKey(DAMAGE_KEY))
        {
            stats.damage = PlayerPrefs.GetInt(DAMAGE_KEY);
        }
        if (PlayerPrefs.HasKey(MOVE_SPEED_KEY))
        {
            stats.moveSpeed = PlayerPrefs.GetFloat(MOVE_SPEED_KEY);
        }
        if (PlayerPrefs.HasKey(ATTACK_SPEED_KEY))
        {
            stats.attackSpeed = PlayerPrefs.GetFloat(ATTACK_SPEED_KEY);
        }
        if (PlayerPrefs.HasKey(HP_KEY))
        {
            stats.maxHp = PlayerPrefs.GetInt(HP_KEY);
        }

    }

    public void ResetCoin()
    {
        coinCount = 0;
        coinText.text = "内牢 : " + coinCount.ToString();
    }
}
