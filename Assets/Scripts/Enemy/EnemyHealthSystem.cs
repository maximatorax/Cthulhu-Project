using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthSystem : MonoBehaviour, IHealthSystem
{

    public int currentHealth;
    public int maxHealth;
    public bool isInvincible;
    public int invincibleTime;
    public int expToGive;
    public Attack.attackElement elementResistance;
    public Attack.attackElement elementWeakness;
    public Scrollbar HealthBar;
    public TMP_Text HealthBarText;

    private EnemyController enemyController;
    private Animator enemyAnimator;
    private GameObject enemy;
    private float YOffset;
    private Vector3 wantedPosition;

    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = gameObject.GetComponent<Animator>();
        enemy = gameObject;
        currentHealth = maxHealth;
        isInvincible = false;
        enemyController = gameObject.GetComponent<EnemyController>();
        HealthBarText = HealthBar.GetComponentInChildren<TMP_Text>();
        YOffset = (HealthBar.transform.position.y - enemy.transform.position.y)/2;
    }

    // Update is called once per frame
    void Update()
    {
        HealthBar.size = (float) currentHealth / (float) maxHealth;
        HealthBarText.text = currentHealth + "/" + maxHealth;
        wantedPosition = Camera.main.WorldToScreenPoint(enemy.transform.position);
        HealthBar.transform.position = wantedPosition + new Vector3(0, YOffset, 0);
    }

    public void TakeDamage(int damage)
    {
        if (isInvincible) return;

        StartCoroutine(doDamage(damage));
    }

    public IEnumerator doDamage(int damage)
    {
        enemyAnimator.SetTrigger("GetHit");
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
        enemyAnimator.SetTrigger("Die");
        enemy.GetComponentInChildren<CapsuleCollider>().enabled = false;
        HealthBar.gameObject.SetActive(false);
        GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStatsSystem>().exp += expToGive;
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
        return;
    }
}
