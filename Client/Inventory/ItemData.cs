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
    [Tooltip("Amount to add to inventory")]
    [SerializeField] private int addAmount;
    [Tooltip("Amount to add/remove to x system per use")]
    [SerializeField] private int itemUseAmount;

    [Header("Weapon Logic")]
    [Tooltip("Applicable if a weapon")]
    [SerializeField] private WeaponData weaponData;


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
    public int ItemUseAmount
    {
        get => ItemUseAmount;
        private set => ItemUseAmount = value;
    }


    // Item Functions //

    /// <summary>
    /// Adds health to the user
    /// </summary>
    public void AddHealth()
    {
        if (type == ItemType.Health)
        {
            HealthManager hm = InstanceFinder.Health_Manager();
            MainInventory mi = InstanceFinder.Main_Inventory();
            hm.AddHealth(itemUseAmount);
            mi.RemoveItem(this, itemUseAmount);         // maybe not needed
        }
    }

    /// <summary>
    /// Removes ammo from the main inventory
    /// </summary>
    public void UseAmmo()
    {
        if (type == ItemType.Ammo)
        {
            MainInventory mi = InstanceFinder.Main_Inventory();
            mi.RemoveItem(this, itemUseAmount);
        }
    }
}