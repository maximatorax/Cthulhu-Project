using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IHealthSystem
{

    void TakeDamage(int damage);
    IEnumerator doDamage(int damage);

    void Die();
    void Heal(int healing);
    void UpdateHealth();
}
