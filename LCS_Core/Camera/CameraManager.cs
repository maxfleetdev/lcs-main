using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls which SceneCamera to active depending on their BoxTrigger conditions. Stores ID's in a dynamic
/// list.
/// </summary>
public class CameraManager : MonoBehaviour
{
    private SceneCamera activeCamera = null;
    private List<int> activeCameras = new List<int>();

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
            activeCamera = null;
            return;
        }
        int active_id = activeCameras[activeCameras.Count - 1];
        activeCamera = CameraCache.GetCamera(active_id);

        // TEST
        Camera.main.transform.position = activeCamera.transform.position;
        Camera.main.transform.rotation = activeCamera.transform.rotation;
    }

    #endregion
}