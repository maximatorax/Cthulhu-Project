using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

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
    public List<int> itemSlot;
    public int additionalSlot;
    public GameObject Dropable;
    public string itemDescription;
}
