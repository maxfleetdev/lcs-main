using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls which SceneCamera to active depending on their Trigger conditions. Stores ID's in a dynamic
/// list.
/// </summary>
public class CameraManager : MonoBehaviour
{
    // Dynamic Camera List
    private readonly List<int> activeCameras = new List<int>();

    #region Startup

    private void OnEnable()
    {
        CacheAllCameras();
        CameraSwitchHandler.OnActivateCamera += ActivateCamera;
        CameraSwitchHandler.OnDeactivateCamera += DeactivateCamera;
    }

    private void OnDisable()
    {
        CameraSwitchHandler.OnActivateCamera -= ActivateCamera;
        CameraSwitchHandler.OnDeactivateCamera -= DeactivateCamera;
    }

    private void CacheAllCameras()
    {
        SceneCamera[] cameras = FindObjectsByType<SceneCamera>(FindObjectsSortMode.None);
        CameraCache.CacheCameras(cameras);
    }

    #endregion

    #region Camera Activation

    private void ActivateCamera(int id, bool nested)
    {
        if (CameraCache.GetCamera(id) != null && !activeCameras.Contains(id))
        {
            // If main camera, clear cameralist and make this ID the active main camera
            // Otherwise, just add to the cameralist
            if (!nested)
            {
                activeCameras.Clear();
            }
            activeCameras.Add(id);
        }
        SetActiveCamera();
    }

    private void DeactivateCamera(int id)
    {
        if (activeCameras.Contains(id))
        {
            activeCameras.Remove(id);
        }
        SetActiveCamera();
    }

    private void SetActiveCamera()
    {
        if (activeCameras.Count == 0)
        {
            Debugger.LogConsole("No Active Camera!", 1);
            return;
        }
        // Makes the most recently added camera the active camera via CameraHandler
        int active_id = activeCameras[^1];
        CameraSwitchHandler.CameraChanged(active_id);
    }

    #endregion
}