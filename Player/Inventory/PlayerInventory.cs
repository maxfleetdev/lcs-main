using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour, IDataPersistence
{
    private List<Slot> inventorySlots = new List<Slot>();
    private int maxAmount = 99, bufferAmount;
    private ItemData bufferItem = null;

    #region Start/Stop

    private void OnEnable()
    {
        InventoryHandler.OnItemPickup += AddToInventory;
        InventoryHandler.OnItemRemove += RemoveFromInventory;
        InventoryHandler.OnInventoryRequest += GetAllSlots;
    }

    private void OnDisable()
    {
        InventoryHandler.OnItemPickup -= AddToInventory;
        InventoryHandler.OnItemRemove -= RemoveFromInventory;
        InventoryHandler.OnInventoryRequest -= GetAllSlots;
    }

    #endregion

    #region Data Persistence

    public void LoadData(GameData data)
    {
        inventorySlots.Clear();
        inventorySlots = data.Inventory;
    }

    public void SaveData(GameData data)
    {
        data.Inventory = inventorySlots;
    }

    #endregion

    #region Inventory Logic

    // Adds to inventory or asks user for pickup choice
    private void AddToInventory(int id, int amount)
    {
        // Check if item exists
        ItemData item = ItemCache.RetrieveItem(id);
        bufferItem = item;
        bufferAmount = amount;
        if (item == null)
            return;

        if (InventoryHasSlot(id))
        {
            // Check if can stack
            if (!item.CanStack)
            {
                Debugger.LogConsole($"Unable to add ID {id} (Can't Stack)", 0);
                InventoryHandler.PickupAction(false);
                return;
            }

            // Check slot validity
            Slot slot = FindSlot(id);
            if (slot == null) return;

            // Limit slot amount
            slot.Amount += amount;
            if (slot.Amount >= maxAmount)
            {
                slot.Amount = maxAmount;
            }
            Debugger.LogConsole($"Added {amount} of ID {id} | {slot.Amount}", 0);
            InventoryHandler.PickupAction(true);
        }

        else
        {
            // Waits for GUI_Pickup response
            GUIHandler.ShowGUI(GUIType.GUI_PICKUP);
            InventoryHandler.OnPickupChoice += PickupChoice;
        }
    }

    // Execute after repsonse from GUI_Pickup
    private void PickupChoice(bool choice)
    {
        if (choice)
        {
            if (!bufferItem.CanStack)
            {
                bufferAmount = 1;
            }
            inventorySlots.Add(new Slot(bufferItem.ItemID, bufferAmount));
            Debugger.LogConsole($"Added Slot of ID {bufferItem.ItemID}", 0);
        }
        InventoryHandler.OnPickupChoice -= PickupChoice;
    }

    // Removes instantly if available
    private void RemoveFromInventory(int id, int amount)
    {
        if (!ItemCache.ItemExists(id))
            return;

        if (InventoryHasSlot(id))
        {
            // Check slot validity
            Slot slot = FindSlot(id);
            if (slot == null) return;

            // Limit slot amount
            slot.Amount -= amount;
            if (slot.Amount <= 0)
            {
                inventorySlots.Remove(slot);
                Debugger.LogConsole($"Removed Slot of ID {id}", 0);
                return;
            }
            Debugger.LogConsole($"Removed {amount} of ID {id} | {slot.Amount}", 0);
        }
    }

    #endregion

    #region Inventory Algorithms

    private bool InventoryHasSlot(int id)
    {
        foreach (Slot slot in inventorySlots)
        {
            if (slot.ItemID == id)
            {
                return true;
            }
        }
        return false;
    }

    private Slot FindSlot(int id)
    {
        foreach (Slot slot in inventorySlots)
        {
            if (slot.ItemID == id)
            {
                return slot;
            }
        }
        return null;
    }

    private void GetAllSlots()
    {
        InventoryHandler.AllInventoryReply(inventorySlots.ToArray());
    }

    #endregion
}