using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.UI;

public class PlayerAttackSystem : MonoBehaviour, IAttackSystem
{
    private Animator playerAnimator;
    private PlayerController Player;
    private CharacterController charController;
    private PlayerStatsSystem playerStatsSystem;
    private PlayerHealthSystem playerHealthSystem;
    private PlayerInventorySystem playerInventorySystem;

    public List<Attack> attackList;
    public LayerMask attackLayer;
    public Attack selectedAttack;
    public int currentAttack;
    public List<Image> attackIcons;

    public int mana;
    public int maxMana;
    public int manaRegenRate;
    [Range(0.1f, 3)]
    public float manaRegenTime;
    public int stamina;
    public int maxStamina;
    public int staminaRegenRate;
    [Range(0.1f, 3)]
    public float staminaRegenTime;
    [HideInInspector]
    public bool isRegenStamina = false;
    [HideInInspector]
    public bool isRegenMana = false;

    public Scrollbar ManaBar;
    private TMP_Text ManaText;
    public Scrollbar StaminaBar;
    private TMP_Text StaminaText;



    void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        Player = gameObject.GetComponent<PlayerController>();
        charController = gameObject.GetComponent<CharacterController>();
        playerStatsSystem = gameObject.GetComponent<PlayerStatsSystem>();
        playerHealthSystem = gameObject.GetComponent<PlayerHealthSystem>();
        ManaText = ManaBar.GetComponentInChildren<TMP_Text>();
        StaminaText = StaminaBar.GetComponentInChildren<TMP_Text>();
        playerInventorySystem = gameObject.GetComponent<PlayerInventorySystem>();
        maxMana = playerStatsSystem.Wisdom * 100;
        mana = maxMana;
        maxStamina = playerStatsSystem.Agility * 100;
        stamina = maxStamina;
        selectedAttack = attackList[0];
        currentAttack = 0;
        foreach (Attack attack in attackList)
        {
            attack.canDo = true;
        }

        for (int x = 0; x < attackList.Count; x++)
        {
            attackIcons[x].sprite = attackList[x].attackIcon;
        }

        attackIcons[currentAttack].color = new Color(attackIcons[currentAttack].color.r, attackIcons[currentAttack].color.g, attackIcons[currentAttack].color.b, 1);
    }

    void Update()
    {
        if(playerStatsSystem.leveling)return;

        if (stamina < maxStamina && !isRegenStamina && !Player.isSprinting)
        {
            StartCoroutine(staminaRegen());
        }

        if (mana < maxMana && !isRegenMana)
        {
            StartCoroutine(manaRegen());
        }

        ManaBar.size = (float)mana / (float)maxMana;
        ManaText.text = mana + "/" + maxMana;
        StaminaBar.size = (float)stamina / (float)maxStamina;
        StaminaText.text = stamina + "/" + maxStamina;

        if (Input.GetButton("Fire1"))
        {
            Attack(selectedAttack);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            attackIcons[currentAttack].color = new Color(attackIcons[currentAttack].color.r, attackIcons[currentAttack].color.g, attackIcons[currentAttack].color.b, 0.5f);
            selectedAttack = GetAttack(0);
            currentAttack = 0;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            attackIcons[currentAttack].color = new Color(attackIcons[currentAttack].color.r, attackIcons[currentAttack].color.g, attackIcons[currentAttack].color.b, 0.5f);
            selectedAttack = GetAttack(1);
            currentAttack = 1;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            attackIcons[currentAttack].color = new Color(attackIcons[currentAttack].color.r, attackIcons[currentAttack].color.g, attackIcons[currentAttack].color.b, 0.5f);
            selectedAttack = GetAttack(2);
            currentAttack = 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            attackIcons[currentAttack].color = new Color(attackIcons[currentAttack].color.r, attackIcons[currentAttack].color.g, attackIcons[currentAttack].color.b, 0.5f);
            selectedAttack = GetAttack(3);
            currentAttack = 3;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            attackIcons[currentAttack].color = new Color(attackIcons[currentAttack].color.r, attackIcons[currentAttack].color.g, attackIcons[currentAttack].color.b, 0.5f);
            selectedAttack = GetAttack(4);
            currentAttack = 4;
        }
        if (Input.mouseScrollDelta.y > 0.0f)
        {
            attackIcons[currentAttack].color = new Color(attackIcons[currentAttack].color.r, attackIcons[currentAttack].color.g, attackIcons[currentAttack].color.b, 0.5f);
            currentAttack++;
            if (currentAttack > 4)
            {
                currentAttack = 4;
            }
            selectedAttack = attackList[currentAttack];
        }
        if (Input.mouseScrollDelta.y < 0.0f)
        {
            attackIcons[currentAttack].color = new Color(attackIcons[currentAttack].color.r, attackIcons[currentAttack].color.g, attackIcons[currentAttack].color.b, 0.5f);
            currentAttack--;
            if (currentAttack < 0)
            {
                currentAttack = 0;
            }
            selectedAttack = attackList[currentAttack];
        }

        attackIcons[currentAttack].color = new Color(attackIcons[currentAttack].color.r, attackIcons[currentAttack].color.g, attackIcons[currentAttack].color.b, 1);
    }

    public void Attack(Attack attack)
    {
        if(!attack.canDo) return;
        if (attack.resssourceUsed == global::Attack.ressource.Mana && attack.ressourceCost > mana)
        {
            Debug.Log("Not enough mana!");
            return;
        }
        else if (attack.resssourceUsed == global::Attack.ressource.Stamina && attack.ressourceCost > stamina)
        {
            Debug.Log("Not enough stamina!");
            return;
        }
        else if (attack.resssourceUsed == global::Attack.ressource.Health &&
                 attack.ressourceCost >= playerHealthSystem.currentHealth)
        {
            Debug.Log("Not enough life!");
            return;
        }

        StartCoroutine(doAttack(attack));
    }

    public IEnumerator doAttack(Attack attack)
    {
        if (attack.resssourceUsed == global::Attack.ressource.Stamina)
            stamina -= attack.ressourceCost;
        else if (attack.resssourceUsed == global::Attack.ressource.Mana)
            mana -= attack.ressourceCost;
        else if (attack.resssourceUsed == global::Attack.ressource.Health)
            playerHealthSystem.currentHealth -= attack.ressourceCost;

        attack.canDo = false;
        playerAnimator.SetTrigger(attack.triggerName);
        Vector3 p1 = transform.position + charController.center + Vector3.up * -charController.height * 0.5f;
        Vector3 p2 = p1 + Vector3.up * charController.height;
        RaycastHit[] hit;
        hit = Physics.CapsuleCastAll(p1, p2, attack.width, transform.forward, attack.range, attackLayer);
        
            foreach (RaycastHit enemy in hit)
            {
                enemy.collider.gameObject.GetComponentInParent<EnemyHealthSystem>().TakeDamage(CalculateDamage(attack, enemy));
            }
            
        yield return new WaitForSeconds(attack.cooldown);
        attack.canDo = true;
    }

    public int CalculateDamage(Attack attack, RaycastHit enemyHit)
    {
        int returnedDamage = attack.damage;

        if (attack.type == global::Attack.attackType.Physical)
        {
            returnedDamage += (int)(((1.0f / 10.0f) * playerStatsSystem.Strength) * returnedDamage);
        }
        else if (attack.type == global::Attack.attackType.Magical)
        {
            returnedDamage += (int)(((1.0f / 10.0f) * playerStatsSystem.Intelligence) * returnedDamage);
        }
        else if (attack.type == global::Attack.attackType.Special)
        {
            returnedDamage *= ((playerStatsSystem.Strength + playerStatsSystem.Intelligence) * returnedDamage);
        }

        if (attack.element == enemyHit.collider.gameObject.GetComponentInParent<EnemyHealthSystem>().elementResistance)
        {
            returnedDamage /= 2;
        }
        else if (attack.element == enemyHit.collider.gameObject.GetComponentInParent<EnemyHealthSystem>().elementWeakness)
        {
            returnedDamage *= 2;
        }

        if (attack.type == enemyHit.collider.gameObject.GetComponentInParent<EnemyHealthSystem>().typeResistance)
        {
            returnedDamage /= 2;
        }
        else if (attack.type == enemyHit.collider.gameObject.GetComponentInParent<EnemyHealthSystem>().typeWeakness)
        {
            returnedDamage *= 2;
        }

        return returnedDamage;
    }

    public Attack GetAttack(int attackNumber)
    {
        return attackList[attackNumber];
    }

    IEnumerator manaRegen()
    {
        isRegenMana = true;
        yield return new WaitForSeconds(2.0f);
        while (mana < maxMana)
        {
            mana += (int)(((float)manaRegenRate /100)*(float)maxMana);
            if (mana > maxMana)
            {
                mana = maxMana;
            }
            yield return new WaitForSeconds(manaRegenTime);
        }

        isRegenMana = false;
    }

    IEnumerator staminaRegen()
    {
        isRegenStamina = true;
        yield return new WaitForSeconds(2.0f);
        while (stamina < maxStamina)
        {
            if (Player.isSprinting)
            {
                isRegenStamina = false;
                yield break;
            }
            stamina += (int) (((float) staminaRegenRate / 100) * (float) maxStamina);
            if (stamina > maxStamina)
            {
                stamina = maxStamina;
            }

            yield return new WaitForSeconds(staminaRegenTime);
        }

        isRegenStamina = false;
    }

    public void RefillMana(int manaToRefill)
    {
        mana += manaToRefill;
        if (mana > maxMana)
        {
            mana = maxMana;
        }
    }

    public void RefillStamina(int staminaToRefill)
    {
        stamina += staminaToRefill;
        if (stamina > maxStamina)
        {
            stamina = maxStamina;
        }
    }

    public void UpdateMana()
    {
        maxMana = playerStatsSystem.Wisdom * 100;
        if (mana > maxMana)
        {
            mana = maxMana;
        }
    }

    public void UpdateStamina()
    {
        maxStamina = playerStatsSystem.Agility * 100;
        if (stamina > maxStamina)
        {
            stamina = maxStamina;
        }
    }
}
