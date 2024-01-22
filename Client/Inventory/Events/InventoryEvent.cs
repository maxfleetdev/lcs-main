using UnityEngine;

[RequireComponent(typeof(ItemHolder))]
public class InventoryEvent : MonoBehaviour
{
    [Tooltip("True = Add; False = Remove")]
    [SerializeField] private bool addToInventory;
    [SerializeField] private int amount;

    public void InventoryChange()
    {
        // Get Info //
        ItemData itemData = GetComponent<ItemHolder>().GetItemData();
        MainInventory inventory = InstanceFinder.Main_Inventory();
        if (amount <= 0)
            amount = 1;

        if (itemData != null && inventory != null)
        {
            switch (addToInventory)
            {
                // Add To Inventory //
                case true:
                    inventory.AddItem(itemData, amount);
                    break;

                // Remove From Inventory //
                case false:
                    inventory.RemoveItem(itemData, amount);
                    break;
            }
        }
    }
}
