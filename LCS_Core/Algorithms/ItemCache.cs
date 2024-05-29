using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Caches all Items found within Resources into a Dictionary. Also Caches recent Items for safer storage
/// </summary>
public static class ItemCache
{
    private static string itemLocation = "Items";
    private static Object[] items;
    private static Dictionary<int, ItemData> cachedItems = new Dictionary<int, ItemData>();
    private static ItemData itemBuffer = null;

    public static void CacheItems()
    {
        cachedItems.Clear();
        items = Resources.LoadAll(itemLocation, typeof(ItemData));
        foreach (ItemData i in items)
        {
            if (cachedItems.ContainsValue(i))
                continue;
            cachedItems.Add(i.ItemID, i);
        }
        Debugger.LogConsole("Cached all Items | Resources/Items", 0);
    }

    public static ItemData RetrieveItem(int id)
    {
        if (cachedItems.Count == 0)
        {
            CacheItems();
        }
        foreach (ItemData i in cachedItems.Values)
        {
            if (i.ItemID == id)
                return i;
        }

        Debugger.LogConsole($"No item exists with ID {id}", 0);
        return null;
    }

    public static bool ItemExists(int id)
    {
        foreach (ItemData i in cachedItems.Values)
        {
            if (i.ItemID == id)
                return true;
        }
        return false;
    }

    /// <summary>
    /// Caches the most recently picked up item by ItemData
    /// </summary>
    /// <param name="id"></param>
    public static void BufferItem(int id)
    {
        itemBuffer = RetrieveItem(id);
    }

    public static ItemData GetBufferedItem()
    {
        return itemBuffer;
    }
}