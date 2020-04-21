using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class PlayerAttackSystem : MonoBehaviour, IAttackSystem
{
    public Animator playerAnimator;
    public PlayerController Player;
    public LayerMask attackLayer;
    public Attack selectedAttack;
    public int currentAttack;
    public List<Image> attackIcons;

    void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        Player = gameObject.GetComponent<PlayerController>();
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
        RaycastHit[] hit;
        hit = Physics.BoxCastAll(transform.position + new Vector3(0, transform.localScale.y / 2, transform.localScale.z), attack.width / 2, transform.TransformDirection(Vector3.forward),
            Quaternion.identity, attack.range, attackLayer);
        
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

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position + new Vector3(0, transform.localScale.y / 2, 0), transform.forward * selectedAttack.range);
        Gizmos.DrawCube(transform.position + new Vector3(0, transform.localScale.y / 2, transform.localScale.z), selectedAttack.width);
    }
}
