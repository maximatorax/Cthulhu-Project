using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Attack")]
public class Attack : ScriptableObject
{
    public int damage;
    public float cooldown;
    public bool canDo;
    public String triggerName;

    public enum attackType
    {
        Normal,
        Special
    };

    public enum attackElement
    {
        Normal,
        Fire,
        Water,
        Ice,
        Lightning,
        Air,
        Ground,
        Light,
        Dark,
        Anime,
        Friendship
    };

    public attackType type;
    public attackElement element;
}
