using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
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
    private bool isShopping = false;
    private GameObject Shopper = null;

    public GameObject inventoryPanel;
    public GameObject equipmentPanel;
    public GameObject InventoryContent;
    public GameObject EquipmentContent;
    public Item[] takenSlots;

    public int money;
    public TMP_Text MoneyText;


    // Start is called before the first frame update
    void Start()
    {
        playerStatsSystem = gameObject.GetComponent<PlayerStatsSystem>();
        playerHealthSystem = gameObject.GetComponent<PlayerHealthSystem>();
        playerAttackSystem = gameObject.GetComponentInChildren<PlayerAttackSystem>();
        charController = gameObject.GetComponent<CharacterController>();
        baseAttack = playerAttackSystem.attackList[0];
        money = 100;
        takenSlots = new Item[10];
    }

    // Update is called once per frame
    void Update()
    {
        if (inventoryPanel.activeSelf)
        {
            MoneyText.text = "Money : " + money.ToString();
        }

        if (Input.GetButtonDown("Inventory") && !inventoryPanel.activeSelf)
        {
            ShowInventory();
        }
        else if (Input.GetButtonDown("Inventory") && inventoryPanel.activeSelf || Input.GetButtonDown("Cancel") && inventoryPanel.activeSelf && !isShopping)
        {
            CloseInventory();
        }
        if (Input.GetButtonDown("Equipment") && !equipmentPanel.activeSelf)
        {
            ShowEquipment();
        }
        else if (Input.GetButtonDown("Equipment") && equipmentPanel.activeSelf || Input.GetButtonDown("Cancel") && equipmentPanel.activeSelf)
        {
            CloseEquipment();
        }

        if (Input.GetButtonDown("Cancel") && isShopping)
        {
            CloseInventory();
            Shopper.GetComponent<Shop>().CloseInventory();
            Shopper.GetComponent<Shop>().Buyer = null;
            Shopper = null;
            isShopping = false;
        }

        if (Input.GetButtonDown("Drop") && inventoryPanel.activeSelf)
        {
            Button[] button = inventoryPanel.GetComponentsInChildren<Button>();
                for (int x = 0; x < button.Length; x++)
                {
                    if (button[x].gameObject == EventSystem.current.currentSelectedGameObject)
                    {
                        Drop(Inventory[x]);
                    }

                }

        }

        if (Input.GetButtonDown("Activate") && inventoryPanel.activeSelf && !isShopping)
        {
            Button[] button = inventoryPanel.GetComponentsInChildren<Button>();
            for (int x = 0; x < button.Length; x++)
            {
                if (button[x].gameObject == EventSystem.current.currentSelectedGameObject)
                {
                    EquipButton(Inventory[x], button[x].gameObject);
                }

            }
        }
        else if (Input.GetButtonDown("Activate") && isShopping)
        {
            Button[] ShopButtons = Shopper.GetComponent<Shop>().shopPanel.GetComponentsInChildren<Button>();
            Button[] InventoryButtons = inventoryPanel.GetComponentsInChildren<Button>();
            for (int x = 0; x < ShopButtons.Length; x++)
            {
                if (ShopButtons[x].gameObject == EventSystem.current.currentSelectedGameObject)
                {
                    Buy(Shopper.GetComponent<Shop>().shopInventory[x]);
                }
            }

            for (int y = 0; y < InventoryButtons.Length; y++)
            {
                if (InventoryButtons[y].gameObject == EventSystem.current.currentSelectedGameObject)
                {
                    Sell(Inventory[y]);
                }
            }
        }
        else if (Input.GetButtonDown("Activate"))
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position + charController.center + Vector3.up * -charController.height * 0.5f,
                transform.forward, out hit, 1.5f))
            {
                if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Grabable"))
                {
                    PickUp(hit);
                }
                else if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Shop"))
                {
                    isShopping = true;
                    ShowInventory();
                    Shopper = hit.collider.transform.parent.gameObject;
                    Shopper.GetComponent<Shop>().ShowInventory();
                    Shopper.GetComponent<Shop>().Buyer = gameObject;
                }
            }
        }
    }

    public void ShowInventory()
    {
        inventoryPanel.SetActive(true);
        foreach (Item item in Inventory)
        {
            if(item == null) continue;
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
        List<Item> itemWithSameSlot = new List<Item>();
        if (Equipment.Count > 0)
        {
            foreach (Item equippedItem in Equipment)
            {
                foreach (int itemSlot in item.itemSlot)
                {
                    foreach (int equippedItemSlot in equippedItem.itemSlot)
                    {
                        
                        if (takenSlots[itemSlot] != null && !itemWithSameSlot.Contains(equippedItem) && takenSlots.ToList().IndexOf(equippedItem) == itemSlot ||
                            takenSlots[itemSlot] != null && !itemWithSameSlot.Contains(equippedItem) && takenSlots.ToList().LastIndexOf(equippedItem) == itemSlot)
                        {
                            if (item.GetType() == typeof(Weapon) && item.additionalSlot != 0)
                            {
                                if (takenSlots[item.additionalSlot] == null)
                                {
                                    takenSlots[item.additionalSlot] = item;
                                }
                                else if(takenSlots[item.additionalSlot].GetType() != typeof(Weapon))
                                {
                                    itemWithSameSlot.Add(takenSlots[item.additionalSlot]);
                                    takenSlots[item.additionalSlot] = item;
                                }
                                else
                                {
                                    itemWithSameSlot.Add(equippedItem);
                                }
                            }
                            else
                            {
                                itemWithSameSlot.Add(equippedItem);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            foreach (int slot in item.itemSlot)
            {
                takenSlots[slot] = item;
            }
        }

        foreach (Item equippedItem in itemWithSameSlot)
        {
            for (int x = 0; x < takenSlots.Length; x++)
            {
                if (takenSlots.Contains(equippedItem))
                {
                    takenSlots[takenSlots.ToList().IndexOf(equippedItem)] = null;
                }
            }
            Unequip(equippedItem);
        }

        foreach (int s in item.itemSlot)
        {
            if (takenSlots[s] == null)
            {
                takenSlots[s] = item;
            }
        }
        Equip(item);
        Destroy(button);
    }

    public void UnequipButton(Item item, GameObject button)
    {
        if (item.GetType() == typeof(Weapon) && takenSlots[item.additionalSlot] != null)
        {
            foreach (int slot in item.itemSlot)
            {
                if (takenSlots[slot] == item)
                {
                    Unequip(item);
                    Destroy(button);
                    takenSlots[item.additionalSlot] = null;
                }
            }
        }
        else
        {
            foreach (int slot in item.itemSlot)
            {
                takenSlots[slot] = null;
            }

            Unequip(item);
            Destroy(button);
        }
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
            if (item == null) continue;
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

    public void PickUp(RaycastHit hit)
    {
        
        AddToInventory(hit.collider.gameObject.GetComponent<Grabable>().assignedItem);
        Destroy(hit.collider.gameObject);
        
    }

    public void Drop(Item itemToDrop)
    {
        Instantiate(itemToDrop.Dropable, transform.position + Vector3.forward, Quaternion.identity);
        DeleteFromInventory(Inventory[Inventory.IndexOf(itemToDrop)]);
    }

    public void Buy(Item itemToBuy)
    {
        if (money >= itemToBuy.value)
        {
            AddToInventory(itemToBuy);
            Shopper.GetComponent<Shop>().Sell(itemToBuy);
            money -= itemToBuy.value;
        }
        else
        {
            Debug.Log("You don't have enough money!");
        }
    }

    public void Sell(Item itemToSell)
    {
        if (Shopper.GetComponent<Shop>().money >= itemToSell.value / 2)
        {
            DeleteFromInventory(itemToSell);
            Shopper.GetComponent<Shop>().Buy(itemToSell);
            money += itemToSell.value / 2;
        }
        else
        {
            Debug.Log("The vendor don't have enough money!");
        }
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

        if (inventoryPanel.activeSelf)
        {
            CloseInventory();
            ShowInventory();
        }
    }

    public void Unequip(Item itemToUnequip)
    {
        Inventory.Add(itemToUnequip);
        Equipment.Remove(itemToUnequip);
        SubItemBonus(itemToUnequip);
        if (itemToUnequip.GetType() == typeof(Weapon))
        {
            SubWeaponAttack((Weapon)itemToUnequip);
            if (Equipment.Any(i => i.GetType() == typeof(Weapon)))
            {
                Item item = Equipment.Find(i => i.GetType() == typeof(Weapon));
                AddWeaponAttack((Weapon)item);
            }
        }

        if (inventoryPanel.activeSelf)
        {
            CloseInventory();
            ShowInventory();
        }

        if (equipmentPanel.activeSelf)
        {
            CloseEquipment();
            ShowEquipment();
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
