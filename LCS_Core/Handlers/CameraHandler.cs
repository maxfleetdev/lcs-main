using System;
using System.Collections.Generic;

public static class CameraHandler
{
    public static event Action<int> OnActivateCamera;
    public static event Action<int> OnCameraChanged;

    private static Dictionary<int, SceneCamera> sceneCameras = new Dictionary<int, SceneCamera>();

    public static void ActivateCamera(int id)
    {
        OnActivateCamera?.Invoke(id);
    }

    public static void CameraChanged(int id)
    {
        OnCameraChanged?.Invoke(id);
    }

    #region Scene Cameras

    public static void SetupSceneCameras(Dictionary<int, SceneCamera> cams)
    {
        if (cams.Count == 0) return;
        sceneCameras = cams;
    }

    public static Dictionary<int, SceneCamera> GetSceneCameras()
    {
        return sceneCameras;
    }

    #endregion
}