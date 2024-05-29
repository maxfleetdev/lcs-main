using TMPro;
using UnityEngine;

public class PickupGUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI dialogueText;
    private ItemData itemData = null;

    #region Start/Stop GUI

    private void OnEnable()
    {
        // Gather from ItemBuffer
        itemData = ItemCache.GetBufferedItem();
        if (itemData == null)
        {
            Debugger.LogConsole("No Item in ItemBuffer!", 1, this);
            this.enabled = false;
        }
        // Setup GUIText
        SetText();
    }

    private void OnDisable()
    {
        dialogueText.text = string.Empty;
    }

    #endregion

    #region GUI Logic

    private void SetText()
    {
        string item_name = itemData.ItemName;
        dialogueText.text = $"Pickup <color={ColorName(itemData.Type)}>" + item_name + "</color>?";
    }

    public void PickupChoice(bool choice)
    {
        InventoryHandler.PickupAction(choice);
        GUIHandler.HideGUI();
    }

    private string ColorName(ItemType type)
    {
        switch (type)
        {
            case ItemType.ITEM_PUZZLE:
                return "purple";
            case ItemType.ITEM_AMMO:
                return "blue";
            case ItemType.ITEM_WEAPON:
                return "red";
            case ItemType.ITEM_HEALTH:
                return "green";
            default:
                return "white";
        }
    }

    #endregion
}