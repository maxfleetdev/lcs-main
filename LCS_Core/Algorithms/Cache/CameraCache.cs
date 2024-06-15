using System.Collections.Generic;

public static class CameraCache
{
    private static Dictionary<int, SceneCamera> cachedCameras = new Dictionary<int, SceneCamera>();

    public static void CacheCameras(SceneCamera[] cameras)
    {
        // Flush Cache
        cachedCameras.Clear();
        if (cameras.Length == 0)
        {
            Debugger.LogConsole("No Virtual Cameras to Cache", 1);
            return;
        }
        int index = 0;
        foreach (var cam in cameras)
        {
            if (cam == null) continue;
            cam.InitialiseCamera(index);
            cachedCameras.Add(index, cam);
            index++;
        }
        Debugger.LogConsole("Cached all Cameras", 0);
    }

    public static SceneCamera GetCamera(int id)
    {
        if (cachedCameras.ContainsKey(id))
        {
            return cachedCameras[id];
        }
        else
        {
            Debugger.LogConsole($"No Camera with ID {id}", 1);
            return null;
        }
    }
}