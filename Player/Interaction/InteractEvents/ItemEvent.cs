using UnityEngine;

public enum InteractType
{
    PICKUP,
    REMOVE
};

public class ItemEvent : MonoBehaviour, IDataPersistence
{
    [SerializeField] private ItemData item;
    [SerializeField] private int amount;
    [Space]
    [SerializeField] private InteractType interactType;
    [Space]
    [SerializeField] private string objectID;

    private bool pickedUp = false;

    #region GUID

    [ContextMenu("Generate GUID")]
    private void GenerateGuid()
    {
        objectID = System.Guid.NewGuid().ToString();
    }

    #endregion

    #region Data Handling

    public void SaveData(GameData data)
    {
        if (data.ObjectsRemoved.Contains(objectID))
        {
            data.ObjectsRemoved.Remove(objectID);
        }

        if (pickedUp)
        {
            data.ObjectsRemoved.Add(objectID);
        }
    }

    public void LoadData(GameData data)
    {
        pickedUp = data.ObjectsRemoved.Contains(objectID) ? true : false;
        ToggleObject();
    }

    #endregion

    #region Interact Logic

    public void OnInteract()
    {
        // Null Checks
        if (objectID == string.Empty)
        {
            Debugger.LogConsole("No GUID found!", 2, this);
        }
        if (item == null || pickedUp) return;

        switch (interactType)
        {
            case InteractType.PICKUP:
                PickupItem();
                break;

            case InteractType.REMOVE:
                RemoveItem();
                break;

            default:
                break;
        }
    }

    private void PickupItem()
    {
        InventoryHandler.OnPickupChoice += PickupChoice;
        item.Pickup(amount);
    }

    private void RemoveItem()
    {
        item.Remove(amount);
    }

    private void PickupChoice(bool choice)
    {
        InventoryHandler.OnPickupChoice -= PickupChoice;
        if (choice)
        {
            pickedUp = true;
            ToggleObject();
        }
    }

    private void ToggleObject()
    {
        // Picked up (true) = Toggle OFF
        GetComponent<MeshRenderer>().enabled = !pickedUp;
        GetComponent<BoxCollider>().enabled = !pickedUp;
    }

    #endregion
}