using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SkillType
{
    Dash,
    Grappling
}

public enum WeaponType
{
    Short,
    Long
}

public class PlayerStats : MonoBehaviour
{
    public static PlayerStats Instance;

    [Header("플레이어 능력치")]
    public int maxHp = 100;
    public int currentHp;
    public int damage = 10;
    public float attackSpeed = 1.0f;
    public float moveSpeed = 3.0f;
    public SkillType skillType = SkillType.Grappling;
    public WeaponType weaponType = WeaponType.Short;
    public bool isDead = false;
    public UIManager uiManager;

    public int killcount = 0;

    private PlayerAnimation animation;

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
        Debug.Log("DataLoad");
        GameManager.instance.LoadCoin();
        GameManager.instance.LoadPlayerStats(this);
        animation = GetComponent<PlayerAnimation>();
    }

    public void Killed()
    {
        killcount++;
    }

    public void ChangeSkill(SkillType skill)
    {
        skillType = skill;
    }

    public void ChangeWeapon(WeaponType weapon)
    {
        weaponType = weapon;
    }

    public void TakeDamage(int amount)
    {
        currentHp -= amount;

        if(currentHp <= 0)
        {
            StartCoroutine(Die());
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

    IEnumerator Die()
    {
        animation.PlayDead();
        isDead = true;
        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Static;

        yield return new WaitForSeconds(2f);
        uiManager.Dead(true);

        yield return new WaitForSeconds(1f);

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().enabled = true;
        isDead = false;
        uiManager.Dead(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //현재 씬 다시 불러오기
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