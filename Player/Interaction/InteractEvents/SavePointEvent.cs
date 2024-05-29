using UnityEngine;

public class SavePointEvent : MonoBehaviour
{
    [SerializeField] private string saveLocation;

    public void OnInteract()
    {
        SaveCache.CacheSaveLocation(saveLocation);          // saves this location name
        GUIHandler.ShowGUI(GUIType.GUI_SAVE_GAME);
    }
}