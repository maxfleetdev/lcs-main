using System;

public static class CameraSwitchHandler
{
    public static event Action<int, bool> OnActivateCamera;
    public static event Action<int> OnDeactivateCamera;
    public static event Action<ViewType> OnForceSwitch;

    public static int previousID;

    // Activate/ Deactivate Calls
    public static void ActivateCamera(int id, bool nested) => OnActivateCamera?.Invoke(id, nested);
    public static void DeactivateCamera(int id) => OnDeactivateCamera?.Invoke(id);

    // Other stuff
    public static void ForceSwitchView(ViewType viewType) => OnForceSwitch?.Invoke(viewType);
}