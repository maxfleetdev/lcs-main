using System;

public static class CameraSwitchHandler
{
    public static event Action<int> OnActivateCamera;
    public static event Action<ViewType> OnForceSwitch;
    public static event Action<int> OnCameraChanged;

    public static void ActivateCamera(int id)
    {
        OnActivateCamera?.Invoke(id);
    }

    public static void CameraChanged(int id)
    {
        OnCameraChanged?.Invoke(id);
    }

    public static void ForceSwitchView(ViewType viewType)
    {
        OnForceSwitch?.Invoke(viewType);
    }
}