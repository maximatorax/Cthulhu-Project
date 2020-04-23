using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerHealthSystem : MonoBehaviour, IHealthSystem
{
    private Animator playerAnimator;
    private PlayerController Player;
    private PlayerStatsSystem playerStatsSystem;

    public Attack.attackElement elementResistance;
    public Attack.attackElement elementWeakness;

    public bool isInvincible;
    public int invincibleTime;

    public int currentHealth;

    public int maxHealth;

    public Scrollbar HealthBar;
    private TMP_Text HealthText;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        Player = gameObject.GetComponent<PlayerController>();
        playerStatsSystem = gameObject.GetComponent<PlayerStatsSystem>();
        HealthText = HealthBar.GetComponentInChildren<TMP_Text>();
        maxHealth = playerStatsSystem.Constitution * 100;
        currentHealth = maxHealth;
        isInvincible = false;
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.size = (float)currentHealth / (float)maxHealth;
        HealthText.text = currentHealth + "/" + maxHealth;
    }

    public void TakeDamage(int damage)
    {
        if(isInvincible) return;

        StartCoroutine(doDamage(damage));
    }

    public IEnumerator doDamage(int damage)
    {
        isInvincible = true;
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
        yield return new WaitForSeconds(invincibleTime);
        isInvincible = false;
    }

    public void Die()
    {
        SceneManager.LoadScene("GameOver");
    }

    public void Heal(int healing)
    {
        currentHealth += healing;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }

    public void UpdateHealth()
    {
        maxHealth = playerStatsSystem.Constitution * 100;
        currentHealth = maxHealth;
    }
}
