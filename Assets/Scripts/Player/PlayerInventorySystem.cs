using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class PlayerInventorySystem : MonoBehaviour, IInventorySystem
{

    public List<Item> Inventory;

    public List<Item> Equipment;

    private PlayerStatsSystem playerStatsSystem;
    private PlayerHealthSystem playerHealthSystem;
    private PlayerAttackSystem playerAttackSystem;
    private Attack baseAttack;

    // Start is called before the first frame update
    void Start()
    {
        playerStatsSystem = gameObject.GetComponent<PlayerStatsSystem>();
        playerHealthSystem = gameObject.GetComponent<PlayerHealthSystem>();
        playerAttackSystem = gameObject.GetComponentInChildren<PlayerAttackSystem>();
        baseAttack = playerAttackSystem.attackList[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Equip(Inventory[0]);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Unequip(Equipment[0]);
        }
    }

    public void ShowInventory()
    {
        throw new System.NotImplementedException();
    }

    public void AddToInventory(Item itemToAdd)
    {
        throw new System.NotImplementedException();
    }

    public void DeleteFromInventory(Item itemToDelete)
    {
        throw new System.NotImplementedException();
    }

    public void PickUp()
    {
        throw new System.NotImplementedException();
    }

    public void Drop()
    {
        throw new System.NotImplementedException();
    }

    public void Equip(Item itemToEquip)
    {
        Equipment.Add(itemToEquip);
        Inventory.Remove(itemToEquip);
        AddItemBonus(itemToEquip);
        if (itemToEquip.GetType() == typeof(Weapon))
        {
            addWeaponAttack((Weapon)itemToEquip);
        }
    }

    public void Unequip(Item itemToUnequip)
    {
        Inventory.Add(itemToUnequip);
        Equipment.Remove(itemToUnequip);
        SubItemBonus(itemToUnequip);
        if (itemToUnequip.GetType() == typeof(Weapon))
        {
            subWeaponAttack((Weapon)itemToUnequip);
        }
    }

    public void AddItemBonus(Item item)
    {
        if (item.StatBonus == Item.statBonus.Strength)
        {
            playerStatsSystem.Strength += item.bonus;
        }
        else if (item.StatBonus == Item.statBonus.Agility)
        {
            playerStatsSystem.Agility += item.bonus;
            playerAttackSystem.UpdateStamina();
        }
        else if (item.StatBonus == Item.statBonus.Constitution)
        {
            playerStatsSystem.Constitution += item.bonus;
            playerHealthSystem.UpdateHealth();
        }
        else if (item.StatBonus == Item.statBonus.Wisdom)
        {
            playerStatsSystem.Wisdom += item.bonus;
            playerAttackSystem.UpdateMana();
        }
        else if (item.StatBonus == Item.statBonus.Intelligence)
        {
            playerStatsSystem.Intelligence += item.bonus;
        }
    }

    public void SubItemBonus(Item item)
    {
        if (item.StatBonus == Item.statBonus.Strength)
        {
            playerStatsSystem.Strength -= item.bonus;
        }
        else if (item.StatBonus == Item.statBonus.Agility)
        {
            playerStatsSystem.Agility -= item.bonus;
            playerAttackSystem.UpdateStamina();
        }
        else if (item.StatBonus == Item.statBonus.Constitution)
        {
            playerStatsSystem.Constitution -= item.bonus;
            playerHealthSystem.UpdateHealth();
        }
        else if (item.StatBonus == Item.statBonus.Wisdom)
        {
            playerStatsSystem.Wisdom -= item.bonus;
            playerAttackSystem.UpdateMana();
        }
        else if (item.StatBonus == Item.statBonus.Intelligence)
        {
            playerStatsSystem.Intelligence -= item.bonus;
        }
    }

    public void addWeaponAttack(Weapon weapon)
    {
        playerAttackSystem.attackList[0] = weapon.weaponAttack;
        playerAttackSystem.attackIcons[0].sprite = weapon.weaponAttack.attackIcon;
    }

    public void subWeaponAttack(Weapon weapon)
    {
        playerAttackSystem.attackList[0] = baseAttack;
        playerAttackSystem.attackIcons[0].sprite = baseAttack.attackIcon;
    }
}
