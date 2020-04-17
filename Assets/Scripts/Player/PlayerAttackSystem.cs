using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackSystem : MonoBehaviour, IAttackSystem
{
    public Animator playerAnimator;
    public PlayerController Player;

    void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        Player = gameObject.GetComponent<PlayerController>();
        foreach (Attack attack in Player.attackList)
        {
            attack.canDo = true;
        }
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Attack(Player.attackList[0]);
        }

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Attack(GetAttack(1));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Attack(GetAttack(2));
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            Attack(GetAttack(3));
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            Attack(GetAttack(4));
        }
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
        Debug.Log(attack);
        yield return new WaitForSeconds(attack.cooldown);
        attack.canDo = true;
    }

    public Attack GetAttack(int attackNumber)
    {
        return Player.attackList[attackNumber];
    }
}
