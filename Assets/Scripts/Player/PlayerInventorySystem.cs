using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInventorySystem : MonoBehaviour, IInventorySystem
{

    public List<Item> Inventory;

    public List<Item> Equipment;

    private PlayerStatsSystem playerStatsSystem;
    private PlayerHealthSystem playerHealthSystem;
    private PlayerAttackSystem playerAttackSystem;
    private Attack baseAttack;

    public GameObject inventoryPanel;
    public GameObject equipmentPanel;
    public GameObject InventoryContent;
    public GameObject EquipmentContent;
    [Range(0, 2)]
    public int nbOfFreeHands;

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
        if (Input.GetButtonDown("Inventory") && !inventoryPanel.activeSelf)
        {
            ShowInventory();
        }
        else if (Input.GetButtonDown("Inventory") && inventoryPanel.activeSelf)
        {
            CloseInventory();
        }
        if (Input.GetButtonDown("Equipment") && !equipmentPanel.activeSelf)
        {
            ShowEquipment();
        }
        else if (Input.GetButtonDown("Equipment") && equipmentPanel.activeSelf)
        {
            CloseEquipment();
        }
    }

    public void ShowInventory()
    {
        inventoryPanel.SetActive(true);
        foreach (Item item in Inventory)
        {
            GameObject go = new GameObject();
            Instantiate(go, Vector3.up, Quaternion.identity);
            go.SetActive(true);
            go.transform.parent = InventoryContent.transform;
            go.AddComponent<Image>();
            go.AddComponent<Button>();
            go.GetComponent<Image>().sprite = item.itemIcon;
            go.GetComponent<Button>().targetGraphic = go.GetComponent<Image>();
            go.GetComponent<Button>().onClick.AddListener(delegate { EquipButton(item, go); });
        }
    }

    public void EquipButton(Item item, GameObject button)
    {
        if (nbOfFreeHands < item.nbOfHands) return;
        Equip(item);
        Destroy(button);
        nbOfFreeHands -= item.nbOfHands;
    }

    public void UnequipButton(Item item, GameObject button)
    {
        Unequip(item);
        Destroy(button);
        nbOfFreeHands += item.nbOfHands;
    }

    public void CloseInventory()
    {
        foreach (Transform child in InventoryContent.transform)
        {
            Destroy(child.gameObject);
        }
        inventoryPanel.SetActive(false);
    }

    public void ShowEquipment()
    {
        equipmentPanel.SetActive(true);
        foreach (Item item in Equipment)
        {
            GameObject go = new GameObject();
            Instantiate(go, Vector3.up, Quaternion.identity);
            go.SetActive(true);
            go.transform.parent = EquipmentContent.transform;
            go.AddComponent<Image>();
            go.AddComponent<Button>();
            go.GetComponent<Image>().sprite = item.itemIcon;
            go.GetComponent<Button>().targetGraphic = go.GetComponent<Image>();
            go.GetComponent<Button>().onClick.AddListener(delegate { UnequipButton(item, go); });
        }
    }

    public void CloseEquipment()
    {
        foreach (Transform child in EquipmentContent.transform)
        {
            Destroy(child.gameObject);
        }
        equipmentPanel.SetActive(false);
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
            AddWeaponAttack((Weapon)itemToEquip);
        }
        CloseEquipment();
        ShowEquipment();
    }

    public void Unequip(Item itemToUnequip)
    {
        Inventory.Add(itemToUnequip);
        Equipment.Remove(itemToUnequip);
        SubItemBonus(itemToUnequip);
        if (itemToUnequip.GetType() == typeof(Weapon) && !Equipment.Contains(itemToUnequip))
        {
            SubWeaponAttack((Weapon)itemToUnequip);
        }
        CloseInventory();
        ShowInventory();
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

    public void AddWeaponAttack(Weapon weapon)
    {
        playerAttackSystem.attackList[0] = weapon.weaponAttack;
        playerAttackSystem.attackIcons[0].sprite = weapon.weaponAttack.attackIcon;
    }

    public void SubWeaponAttack(Weapon weapon)
    {
        playerAttackSystem.attackList[0] = baseAttack;
        playerAttackSystem.attackIcons[0].sprite = baseAttack.attackIcon;
    }
}
