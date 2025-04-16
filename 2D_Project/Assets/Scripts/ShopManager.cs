using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Text damageText;
    public Text coinText;

    //�⺻���׷��̵� ���
    public int baseDamageCost = 25;
    public int baseAttackSpeedCost = 27;
    public int baseMoveSpeedCost = 10;
    public int baseHpCost = 10;

    //���׷��̵� ��ġ
    public int damageUpgradeAmount = 5;
    public float attackSpeedUpgradeAmount = 0.2f;
    public float moveSpeedUpgradeAmount = 0.3f;
    public int hpUpgradeAmount = 10;

    //���׷��̵� Ƚ�� ����
    private int damageUpgradeCount = 0;
    private int attackSpeedUpgradeCount = 0;
    private int moveSpeedUpgradeCount = 0;
    private int hpUpgradeCount = 0;

    //���� ��� ����
    private const int increaseThreshold = 3; //3ȸ�̻��϶� ���� ����
    private const float priceIncreaseRate = 1.5f; //��� * 1.5

    private int defaultMaxHp = 100;
    private int defaultDamage = 10;
    private float defaultAttackSpeed = 1.0f;
    private float defaultMoveSpeed = 3.0f;

    private PlayerStats playerStats;

    void Start()
    {
        playerStats = PlayerStats.Instance;
        UpdateUI();
    }

    private int GetCost(int baseCost, int upgradeCount)
    {
        if (upgradeCount < increaseThreshold)
        {
            return baseCost;
        }

        return Mathf.FloorToInt(baseCost * priceIncreaseRate); //�����������(�Ҽ��� ����)
    }

    public void UpgradeDamage()
    {
        UpdateUI();
        SoundManager.Instance.PlaySFX(SFXType.ItemGet);
        int cost = GetCost(baseDamageCost, damageUpgradeCount);
        if (GameManager.instance.UseCoin(cost))
        {
            playerStats.UpgradeDamage(damageUpgradeAmount);
            damageUpgradeCount++;
        }
    }

    public void UpgradeAttackSpeed()
    {
        UpdateUI();
        int cost = GetCost(baseAttackSpeedCost, attackSpeedUpgradeCount);
        if (GameManager.instance.UseCoin(cost))
        {
            playerStats.UpgradeAttackSpeed(attackSpeedUpgradeAmount);
            attackSpeedUpgradeCount++;
        }
    }

    public void UpgradeMoveSpeed()
    {
        UpdateUI();
        int cost = GetCost(baseMoveSpeedCost, moveSpeedUpgradeCount);
        if (GameManager.instance.UseCoin(cost))
        {
            playerStats.UpgradeMoveSpeed(moveSpeedUpgradeAmount);
            moveSpeedUpgradeCount++;
        }
    }

    public void UpgradeHP()
    {
        UpdateUI();
        int cost = GetCost(baseHpCost, hpUpgradeCount);
        if (GameManager.instance.UseCoin(cost))
        {
            playerStats.UpgradeHp(hpUpgradeAmount);
            hpUpgradeCount++;
        }
    }

    public void ResetStats()
    {
        UpdateUI();
        if (GameManager.instance.UseCoin(100))
        {
            playerStats.damage = defaultDamage;
            playerStats.maxHp = defaultMaxHp;
            playerStats.moveSpeed = defaultMoveSpeed;
            playerStats.attackSpeed = defaultAttackSpeed;

            GameManager.instance.SavePlayerStats(playerStats);
        }
    }

    public void UpdateUI()
    {
        damageText.text = playerStats.damage.ToString();
        coinText.text = GameManager.instance.coinCount.ToString();
    }
}
