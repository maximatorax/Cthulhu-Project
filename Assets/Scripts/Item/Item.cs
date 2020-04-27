using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public abstract class Item : ScriptableObject
{
    public int value;
    public int weight;
    public Sprite itemIcon;
    public enum statBonus
    {
        Strength,
        Agility,
        Constitution,
        Wisdom,
        Intelligence
    }

    public statBonus StatBonus;
    public int bonus;
}

[CreateAssetMenu(menuName = "Items/Weapon")]
public class Weapon : Item
{
    public Attack weaponAttack;
}

[CreateAssetMenu(menuName = "Items/Armor")]
public class Armor : Item
{
    public Attack.attackElement ElementResistence;
    public Attack.attackType TypeResistence;
    public float elementResistBonus;
    public float typeResistBonus;
    public int damageResistBonus;
}
