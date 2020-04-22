using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackSystem : MonoBehaviour, IAttackSystem
{
    private Animator playerAnimator;
    private PlayerController Player;
    private CharacterController charController;
    private PlayerStatsSystem playerStatsSystem;

    public LayerMask attackLayer;
    public Attack selectedAttack;
    public int currentAttack;
    public List<Image> attackIcons;


    void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        Player = gameObject.GetComponent<PlayerController>();
        charController = gameObject.GetComponent<CharacterController>();
        playerStatsSystem = gameObject.GetComponent<PlayerStatsSystem>();
        selectedAttack = Player.attackList[0];
        currentAttack = 0;
        foreach (Attack attack in Player.attackList)
        {
            attack.canDo = true;
        }

        for (int x = 0; x < Player.attackList.Count; x++)
        {
            attackIcons[x].sprite = Player.attackList[x].attackIcon;
        }

        attackIcons[currentAttack].color = new Color(attackIcons[currentAttack].color.r, attackIcons[currentAttack].color.g, attackIcons[currentAttack].color.b, 1);

    }

    void Update()
    {
        if(playerStatsSystem.leveling)return;

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
            selectedAttack = Player.attackList[currentAttack];
        }
        if (Input.mouseScrollDelta.y < 0.0f)
        {
            attackIcons[currentAttack].color = new Color(attackIcons[currentAttack].color.r, attackIcons[currentAttack].color.g, attackIcons[currentAttack].color.b, 0.5f);
            currentAttack--;
            if (currentAttack < 0)
            {
                currentAttack = 0;
            }
            selectedAttack = Player.attackList[currentAttack];
        }

        attackIcons[currentAttack].color = new Color(attackIcons[currentAttack].color.r, attackIcons[currentAttack].color.g, attackIcons[currentAttack].color.b, 1);
    }

    public void Attack(Attack attack)
    {
        if(!attack.canDo) return;
        StartCoroutine(doAttack(attack));
    }

    public IEnumerator doAttack(Attack attack)
    {
        attack.canDo = false;
        playerAnimator.SetTrigger(attack.triggerName);
        Vector3 p1 = transform.position + charController.center + Vector3.up * -charController.height * 0.5f;
        Vector3 p2 = p1 + Vector3.up * charController.height;
        RaycastHit[] hit;
        hit = Physics.CapsuleCastAll(p1, p2, attack.width, transform.forward, attack.range, attackLayer);
        
            foreach (RaycastHit enemy in hit)
            {
                enemy.collider.gameObject.GetComponentInParent<EnemyHealthSystem>().TakeDamage(attack.damage);
            }
            
        yield return new WaitForSeconds(attack.cooldown);
        attack.canDo = true;
    }

    public Attack GetAttack(int attackNumber)
    {
        return Player.attackList[attackNumber];
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + charController.center + Vector3.up * -charController.height * 0.5f, transform.forward * selectedAttack.range);
        Gizmos.DrawRay((transform.position + charController.center + Vector3.up * -charController.height * 0.5f) + Vector3.up * charController.height, transform.forward * selectedAttack.range);
    }
}
