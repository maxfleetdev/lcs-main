using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    private Dictionary<int, SceneCamera> sceneCameras;
    private SceneCamera activeCamera = null;

    #region Startup

    private void OnEnable()
    {
        sceneCameras = new Dictionary<int, SceneCamera>();
        CacheCameras();

        CameraHandler.OnActivateCamera += ActivateCamera;
    }

    private void OnDisable()
    {
        CameraHandler.OnActivateCamera -= ActivateCamera;
    }

    private void CacheCameras()
    {
        SceneCamera[] cameras = FindObjectsByType<SceneCamera>(FindObjectsSortMode.None);
        if (cameras.Length == 0)
        {
            Debugger.LogConsole("No Virtual Cameras in scene!", 1);
            return;
        }
        int index = 0;
        foreach (var cam in cameras)
        {
            if (cam == null) continue;
            cam.InitialiseCamera(index);
            sceneCameras.Add(index, cam);
            index++;
        }
    }

    #endregion

    #region Activation

    private void ActivateCamera(int id)
    {
        if (sceneCameras == null || sceneCameras.Count == 0)
            return;

        if (sceneCameras.ContainsKey(id))
        {
            if (activeCamera != null)
            {
                activeCamera.DeactivateCamera();
            }
            activeCamera = sceneCameras[id];
            activeCamera.ActivateCamera();
        }
    }

    #endregion
}