using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Create Item")]
public class ItemData : ScriptableObject
{
    // Serialised Variables //
    [Header("Item Visuals")]
    [SerializeField] private string itemName;
    [SerializeField] private string itemDescription;
    [SerializeField] private GameObject itemModel;

    [Header("Item Logic")]
    [SerializeField] private ItemType type;
    [SerializeField] private int addAmount;


    // Public Properties //
    public string ItemName
    {
        get => name;
        private set => name = value;
    }
    public string ItemDescription
    {
        get => itemDescription;
        private set => itemDescription = value;
    }
    public GameObject Model
    {
        get => itemModel;
        private set => itemModel = value;
    }
    public ItemType Type
    {
        get => type; 
        private set => type = value;
    }
    public int AddAmount
    {
        get => addAmount; 
        private set => addAmount = value;
    }
}