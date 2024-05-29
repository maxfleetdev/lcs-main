using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Container for all information for Items
/// </summary>
[CreateAssetMenu(menuName = "LCS/Inventory/Item")]
public class ItemData : ScriptableObject
{
    [Header("Item Info")]
    public string ItemName;
    [ResizableTextArea] public string ItemDescription;
    public ItemType Type;
    public bool CanStack;
    public GameObject VisualItem;

    [Header("Cache")]
    [MinValue(0)] public int ItemID;

    #region Item Logic

    public void Pickup(int amount)
    {
        if (Type == ItemType.ITEM_MAP)
        {
            // do GUI stuff instead
            return;
        }
        ItemCache.BufferItem(this.ItemID);
        InventoryHandler.AddItem(this.ItemID, amount);
    }

    public void Remove(int amount)
    {
        InventoryHandler.RemoveItem(this.ItemID, amount);
    }

    #endregion
}