using System;

public static class GUIHandler
{
    // Mono -> GUI Manager //
    public static event Action<GUIType> OnGUIToggle;
    public static event Action OnGUIHide;

    // Displays a certain GUI Screen
    public static void ShowGUI(GUIType type)
    {
        OnGUIToggle?.Invoke(type);
    }

    // Hides the current GUI display
    public static void HideGUI()
    {
        OnGUIHide?.Invoke();
    }
}