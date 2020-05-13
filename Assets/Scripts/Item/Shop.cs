using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour, IInventorySystem
{
    public List<Item> shopInventory;

    public GameObject shopPanel;
    public GameObject ItemIcon;
    public GameObject ShopContent;

    public GameObject Buyer;
    public int money;
    public TMP_Text ShopMoneyText;

    void Update()
    {
        if (shopPanel.activeSelf)
        {
            ShopMoneyText.text = "Money : " + money.ToString();
        }
    }
    public void ShowInventory()
    {
        shopPanel.SetActive(true);
        foreach (Item item in shopInventory)
        {
            if (item == null) continue;
            GameObject go = Instantiate(ItemIcon, Vector3.up, Quaternion.identity);
            go.transform.SetParent(ShopContent.transform);
            go.GetComponent<Image>().sprite = item.itemIcon;
            ColorBlock colorBlock = go.GetComponent<Button>().colors;
            colorBlock.selectedColor = Color.gray;
            go.GetComponent<Button>().colors = colorBlock;
        }
    }

    public void CloseInventory()
    {
        foreach (Transform child in ShopContent.transform)
        {
            Destroy(child.gameObject);
        }
        shopPanel.SetActive(false);
    }

    public void AddToInventory(Item itemToAdd)
    {
        shopInventory.Add(itemToAdd);
        if (shopPanel.activeSelf)
        {
            CloseInventory();
            ShowInventory();
        }
    }

    public void DeleteFromInventory(Item itemToDelete)
    {
        shopInventory.Remove(itemToDelete);
        if (shopPanel.activeSelf)
        {
            CloseInventory();
            ShowInventory();
        }
    }

    public void PickUp(RaycastHit hit)
    {
        if (hit.collider == null) return;
        AddToInventory(hit.collider.gameObject.GetComponent<Grabable>().assignedItem);
        Destroy(hit.collider.gameObject);
    }

    public void Drop(Item itemToDrop)
    {
        Instantiate(itemToDrop.Dropable, transform.position + Vector3.forward, Quaternion.identity);
        DeleteFromInventory(shopInventory[shopInventory.IndexOf(itemToDrop)]);
    }

    public void Buy(Item itemToBuy)
    {
        AddToInventory(itemToBuy);
        money -= itemToBuy.value / 2;
    }

    public void Sell(Item itemToSell)
    {
        DeleteFromInventory(itemToSell);
        money += itemToSell.value;
    }

}
