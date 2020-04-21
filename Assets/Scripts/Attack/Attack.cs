﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Attacks/Attack")]
public class Attack : ScriptableObject
{
    public int damage;
    public float cooldown;
    public bool canDo;
    public String triggerName;
    public float range;
    public Vector3 width;
    public Sprite attackIcon;

    public enum attackType
    {
        Physical,
        Magical,
        Special
    };

    public enum attackElement
    {
        Normal,
        Fire,
        Water,
        Grass,
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
