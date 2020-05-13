using System;
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
    [Range(0, 2)]
    public int nbOfHands;
    public GameObject Dropable;
    public string itemDescription;
}
