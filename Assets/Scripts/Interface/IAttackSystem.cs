using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAttackSystem
{
    void Attack(Attack attack);
    IEnumerator doAttack(Attack attack);
}
