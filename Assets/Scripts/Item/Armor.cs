using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Armor")]
public class Armor : Item
{
    public Attack.attackElement ElementResistence;
    public Attack.attackType TypeResistence;
    public float elementResistBonus;
    public float typeResistBonus;
    public int damageResistBonus;
}
