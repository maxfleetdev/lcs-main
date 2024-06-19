using UnityEngine;

public class FadeGUI : MonoBehaviour, IGUIObject
{
    [SerializeField] private FadeScreen fadeScreen;

    public void DisableGUI()
    {
        fadeScreen.ToggleFade();
    }

    public void EnableGUI()
    {
        fadeScreen.ToggleFade();
    }
}