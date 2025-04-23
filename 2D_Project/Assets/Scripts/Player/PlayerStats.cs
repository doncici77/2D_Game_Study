using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public float moveSpeed = 3.0f;
    public SkillType skillType = SkillType.Grappling;
    public WeaponType weaponType = WeaponType.Short;
    public bool isDead = false;
    public UIManager uiManager;
    public Image fadeImage;
    public Text deathText;
    public bool canPuase = true;
    public bool isPaused = false;

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

    public int TakeDamage(int amount)
    {
        currentHp -= amount;

        if(currentHp <= 0)
        {
            StartCoroutine(Die());
        }

        return currentHp;
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
        SoundManager.Instance.PlaySFX(SFXType.PlayerDead);

        yield return new WaitForSeconds(2f);

        fadeImage.gameObject.SetActive(true);
        deathText.gameObject.SetActive(true);

        float fadeDuration = 2.0f; // 페이드 효과 시간
        float elapsed = 0f;

        // 화면이 검게 변함
        while (elapsed < fadeDuration)
        {
            float alpha = elapsed / fadeDuration;
            fadeImage.color = new Color(0, 0, 0, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }
        fadeImage.color = new Color(0, 0, 0, 1); // 완전 검게

        yield return new WaitForSeconds(0.5f);

        // "YOU DIED" 텍스트 서서히 나타남
        elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            float alpha = elapsed / fadeDuration;
            deathText.color = new Color(1, 0, 0, alpha);
            elapsed += Time.deltaTime;
            yield return null;
        }
        deathText.color = new Color(1, 0, 0, 1); // 완전 빨간색

        yield return new WaitForSeconds(1f);

        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().enabled = true;
        isDead = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); //현재 씬 다시 불러오기
    }

    public int GetDamage()
    {
        return damage;
    }

    public void UpgradeDamage(int amount)
    {
        damage += amount;
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