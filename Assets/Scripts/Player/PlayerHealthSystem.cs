using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealthSystem : MonoBehaviour, IHealthSystem
{

    public Animator playerAnimator;
    public PlayerController Player;
    public bool isInvincible;
    public int invincibleTime;

    public int currentHealth;

    public int maxHealth = 100;

    // Start is called before the first frame update
    void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        Player = gameObject.GetComponent<PlayerController>();
        currentHealth = maxHealth;
        isInvincible = false;
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
