using UnityEngine;

public class PickupScreenGUI : MonoBehaviour, IGUIObject
{
    [SerializeField] private GameObject guiElement;

    public void EnableGUI()
    {
        guiElement.SetActive(true);
    }

    public void DisableGUI()
    {
        guiElement.SetActive(false);
    }
}
