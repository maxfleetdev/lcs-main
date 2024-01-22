using UnityEngine;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    public ItemData GetItemData()
    {
        if (itemData != null)
        {
            return itemData;
        }

        else
        {
            return null;
        }
    }
}
