﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInventorySystem
{
    void ShowInventory();
    void CloseInventory();
    void AddToInventory(Item itemToAdd);
    void DeleteFromInventory(Item itemToDelete);
    void PickUp();
    void Drop(Item itemToDrop);
}
