using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealthSystem : MonoBehaviour, IHealthSystem
{

    public int currentHealth;

    public int maxHealth;
    public EnemyController enemyController;
    public Animator enemyAnimator;
    public GameObject enemy;
    public bool isInvincible;
    public int invincibleTime;
    // Start is called before the first frame update
    void Start()
    {
        enemyAnimator = gameObject.GetComponent<Animator>();
        enemy = gameObject;
        currentHealth = maxHealth;
        isInvincible = false;
        enemyController = gameObject.GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

    public void Heal(int healing)
    {
        currentHealth += healing;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
}
