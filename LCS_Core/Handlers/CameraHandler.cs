using System;

public static class CameraHandler
{
    public static event Action<int> OnActivateCamera;

    public static void ActivateCamera(int id)
    {
        OnActivateCamera?.Invoke(id);
    }
}