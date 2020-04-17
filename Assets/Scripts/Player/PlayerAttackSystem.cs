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
    }

    void Update()
    {
        if (Input.GetButton("Fire1"))
        {
            Attack(Player.attackList[0]);
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
        Debug.Log(attack.damage);
        yield return new WaitForSeconds(attack.cooldown);
        attack.canDo = true;
    }
}
