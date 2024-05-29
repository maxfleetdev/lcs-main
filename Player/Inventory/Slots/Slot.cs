/// <summary>
/// Serialised Slot which is used for saving/loading data. Main use in Inventory (Item ID and Quantity)
/// </summary>
[System.Serializable]
public class Slot
{
    public int Amount;
    public int ItemID;

    public Slot(int item_id, int amount)
    {
        this.Amount = amount;
        this.ItemID = item_id;
    }
}