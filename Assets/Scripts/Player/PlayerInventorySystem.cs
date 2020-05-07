using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Image = UnityEngine.UI.Image;

public class PlayerInventorySystem : MonoBehaviour, IInventorySystem
{

    public List<Item> Inventory;

    public List<Item> Equipment;

    public GameObject ItemIcon;

    private PlayerStatsSystem playerStatsSystem;
    private PlayerHealthSystem playerHealthSystem;
    private PlayerAttackSystem playerAttackSystem;
    private CharacterController charController;
    private Attack baseAttack;

    public GameObject inventoryPanel;
    public GameObject equipmentPanel;
    public GameObject InventoryContent;
    public GameObject EquipmentContent;
    [Range(0, 2)]
    public int nbOfFreeHands;

    public LayerMask grabableLayer;

    // Start is called before the first frame update
    void Start()
    {
        playerStatsSystem = gameObject.GetComponent<PlayerStatsSystem>();
        playerHealthSystem = gameObject.GetComponent<PlayerHealthSystem>();
        playerAttackSystem = gameObject.GetComponentInChildren<PlayerAttackSystem>();
        charController = gameObject.GetComponent<CharacterController>();
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

        if (Input.GetButtonDown("Drop") && inventoryPanel.activeSelf)
        {
            Button[] button = inventoryPanel.GetComponentsInChildren<Button>();
                for (int x = 0; x < inventoryPanel.GetComponentsInChildren<Button>().Length; x++)
                {
                    if (button[x].gameObject == EventSystem.current.currentSelectedGameObject)
                    {
                        Drop(Inventory[x]);
                    }

                }

        }
        else if (Input.GetButtonDown("Activate") && inventoryPanel.activeSelf)
        {
            Button[] button = inventoryPanel.GetComponentsInChildren<Button>();
            for (int x = 0; x < inventoryPanel.GetComponentsInChildren<Button>().Length; x++)
            {
                if (button[x].gameObject == EventSystem.current.currentSelectedGameObject)
                {
                    EquipButton(Inventory[x], button[x].gameObject);
                }

            }
        }
        else if (Input.GetButtonDown("Activate"))
        {
            PickUp();
        }
    }

    public void ShowInventory()
    {
        inventoryPanel.SetActive(true);
        foreach (Item item in Inventory)
        {
            GameObject go = Instantiate(ItemIcon, Vector3.up, Quaternion.identity);
            go.transform.SetParent(InventoryContent.transform);
            go.GetComponent<Image>().sprite = item.itemIcon;
            ColorBlock colorBlock = go.GetComponent<Button>().colors;
            colorBlock.selectedColor = Color.gray;
            go.GetComponent<Button>().colors = colorBlock;
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
            GameObject go = Instantiate(ItemIcon, Vector3.up, Quaternion.identity);
            go.transform.SetParent(EquipmentContent.transform);
            go.GetComponent<Image>().sprite = item.itemIcon;
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
        Inventory.Add(itemToAdd);
        if (inventoryPanel.activeSelf)
        {
            CloseInventory();
            ShowInventory();
        }
    }

    public void DeleteFromInventory(Item itemToDelete)
    {
        Inventory.Remove(itemToDelete);
        if (inventoryPanel.activeSelf)
        {
            CloseInventory();
            ShowInventory();
        }
    }

    public void PickUp()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position + charController.center + Vector3.up * -charController.height * 0.5f,
            transform.forward, out hit, 1.5f, grabableLayer))
        {
            AddToInventory(hit.collider.gameObject.GetComponent<Grabable>().assignedItem);
            Destroy(hit.collider.gameObject);
        }
    }

    public void Drop(Item itemToDrop)
    {
        Instantiate(itemToDrop.Dropable, transform.position + Vector3.forward, Quaternion.identity);
        DeleteFromInventory(Inventory[Inventory.IndexOf(itemToDrop)]);
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

        if (equipmentPanel.activeSelf)
        {
            CloseEquipment();
            ShowEquipment();
        }
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

        if (inventoryPanel.activeSelf)
        {
            CloseInventory();
            ShowInventory();
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
