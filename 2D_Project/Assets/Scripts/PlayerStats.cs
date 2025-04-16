using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("플레이어 능력치")]
    public int maxHp = 100;
    public int currentHp;
    public int damage = 10;
    public float attackSpeed = 1.0f;
    public float moveSpeed = 3.0f;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        currentHp = maxHp;
    }

    void Start()
    {
        GameManager.instance.LoadPlayerStats(this);
    }

    public void TakeDamage(int amount)
    {
        SoundManager.Instance.PlaySFX(SFXType.TakeDamage);
        // 애니메이션 재생
        currentHp -= amount;

        if(currentHp <= 0)
        {
            // 죽음
        }
    }

    public void Heal(int amount)
    {
        currentHp += amount;

        if(currentHp > maxHp)
        {
            currentHp = maxHp;
        }
    }

    public void Die()
    {
        // 예) GameOver
    }

    public int GetDamage()
    {
        return damage;
    }

    public float GetAttackSpeed()
    {
        return attackSpeed;
    }

    public void UpgradeDamage(int amount)
    {
        damage += amount;
        GameManager.instance.SavePlayerStats(this);
    }

    public void UpgradeAttackSpeed(float amount)
    {
        attackSpeed += amount;
        GameManager.instance.SavePlayerStats(this);
    }

    public void UpgradeHp(int amount)
    {
        maxHp += amount;
        GameManager.instance.SavePlayerStats(this);
    }

    public void UpgradeMoveSpeed(float amount)
    {
        moveSpeed += amount;
        GameManager.instance.SavePlayerStats(this);
    }
}