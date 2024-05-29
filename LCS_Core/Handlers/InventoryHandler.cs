using System;

/// <summary>
/// Main handler for passing events to Inventory Listeners
/// </summary>
public static class InventoryHandler
{
    /// <summary>
    /// ItemID and Amount to Add
    /// </summary>
    public static event Action<int, int> OnItemPickup;

    /// <summary>
    /// ItemID and Amount to Remove
    /// </summary>
    public static event Action<int, int> OnItemRemove;

    /// <summary>
    /// Called upon GUI Response
    /// </summary>
    public static event Action<bool> OnPickupChoice;

    // ItemData -> Inventory
    public static void AddItem(int item_id, int amount)
    {
        if (amount <= 0)
        {
            Debugger.LogConsole("Cannot add less than 0", 1);
            return;
        }
        OnItemPickup?.Invoke(item_id, amount);
    }

    // ItemData -> Inventory
    public static void RemoveItem(int item_id, int amount)
    {
        if (amount < 0)
        {
            if (amount == 0)
            {
                Debugger.LogConsole("Cannot remove 0", 1);
                return;
            }
            amount = Math.Abs(amount);
        }
        OnItemRemove?.Invoke(item_id, amount);
    }

    // GUI -> Inventory
    public static void PickupAction(bool choice)
    {
        OnPickupChoice?.Invoke(choice);
    }
}