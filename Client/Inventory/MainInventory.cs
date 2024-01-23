using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainInventory : MonoBehaviour
{
    #region Class Variables

    // Protected Vars //
    protected private static MainInventory instance;
    protected private int invSpace = 6;     // amount of spaces in inventory
    protected static private Dictionary<ItemData, int> inventory = new Dictionary<ItemData, int>();      // initialise

    // Public Properties //
    public static MainInventory Instance
    {
        get => instance;
        private set => instance = value;
    }
    public static Dictionary<ItemData, int> Inventory
    {
        get => inventory;
        private set => inventory = value;
    }

    // Callback //
    public Action OnChange;

    #endregion

    #region Startup/Shutdown

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            DebugSystem.Log("Inventory already instantiated!", LogType.Warn);
            Destroy(gameObject);
        }
        
        LoadInventory();
    }

    private void OnDisable()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    private void LoadInventory()
    {
        // Load inventory data here if in a saved game
        // Otherwise load a fresh inventory
        inventory.Clear();
        OnChange?.Invoke();
    }

    #endregion

    #region Inventory Logic

    // PUBLIC LOGIC //

    /// <summary>
    /// Adds the passed value to Inventory. Pass amount for custom add amount
    /// </summary>
    /// <param name="data"></param>
    public void AddItem(ItemData data, int amount = 1)
    {
        if (inventory.ContainsKey(data))
        {
            AddAmount(data, amount);
        }

        else if (CheckSpace())
        {
            DebugSystem.Log($"Added {data.ItemName} by {amount}", LogType.Debug);
            inventory.Add(data, amount);
        }

        else
        {
            DebugSystem.Log($"Unable to add: {data.ItemName}! Not enough space", LogType.Warn);
            return;
        }

        // public callback
        OnChange?.Invoke();
    }

    /// <summary>
    /// Removes the passed the value from Inventory. Pass amount for custom remove amount
    /// </summary>
    /// <param name="data"></param>
    public void RemoveItem(ItemData data, int amount = 1)
    {
        if (inventory.ContainsKey(data))
        {
            // only remove 1
            int diff = inventory[data] - amount;
            if (diff >= 1)
            {
                RemoveAmount(data, amount);
            }

            // remove item
            else
            {
                inventory.Remove(data);
            }
            DebugSystem.Log($"Removed {data.ItemName} by {amount}", LogType.Debug);
        }

        else
        {
            DebugSystem.Log($"Unable to remove: {data.ItemName}! Not in Inventory", LogType.Warn);
            return;
        }

        // public callback
        OnChange?.Invoke();
    }


    // PROTECTED LOGIC //

    /// <summary>
    /// Adds x quantity of an item
    /// </summary>
    /// <param name="data"></param>
    /// <param name="amount"></param>
    private protected void AddAmount(ItemData data, int amount)
    {
        inventory[data] += amount;
        DebugSystem.Log($"Added {data.ItemName}, now: {inventory[data]}", LogType.Debug);
    }

    /// <summary>
    /// Removes x quantity of an item
    /// </summary>
    /// <param name="data"></param>
    /// <param name="amount"></param>
    private protected void RemoveAmount(ItemData data, int amount)
    {
        inventory[data] -= amount;
        DebugSystem.Log($"Removed {data.ItemName}, now: {inventory[data]}", LogType.Debug);
    }

    /// <summary>
    /// Returns True if enough space inside inventory
    /// </summary>
    /// <returns></returns>
    private protected bool CheckSpace()
    {
        // enough space
        if (inventory.Count < invSpace)
        {
            return true;
        }

        // nope
        else
        {
            return false;
        }
    }

    #endregion
}