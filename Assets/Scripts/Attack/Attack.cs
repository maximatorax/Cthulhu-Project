using System;
using UnityEngine;

[CreateAssetMenu(menuName = "Attacks/Attack")]
public class Attack : ScriptableObject
{
    public int damage;
    public float cooldown;
    public bool canDo;
    public String triggerName;
    public float range;
    public float width;
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

    public enum ressource
    {
        Mana,
        Stamina,
        Health
    }

    public attackType type;
    public attackElement element;
    public ressource resssourceUsed;
    public int ressourceCost;
}
