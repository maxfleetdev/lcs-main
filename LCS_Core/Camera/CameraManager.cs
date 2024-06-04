using UnityEngine;

public class CameraManager : MonoBehaviour
{
    // Camera Switching
    private SceneCamera activeCamera = null;
    private CameraData currentCameraData;
    private ViewType currentViewType;

    #region Startup

    private void OnEnable()
    {
        // Scene Cameras
        CacheAllCameras();
        CameraHandler.OnActivateCamera += ActivateCamera;
    }

    private void OnDisable()
    {
        CameraHandler.OnActivateCamera -= ActivateCamera;
    }

    private void CacheAllCameras()
    {
        // Uses cache rather than this class
        SceneCamera[] cameras = FindObjectsByType<SceneCamera>(FindObjectsSortMode.None);
        CameraCache.CacheCameras(cameras);
    }

    #endregion

    #region Camera Activation

    private void ActivateCamera(int id)
    {
        SceneCamera camera = CameraCache.GetCamera(id);
        if (camera == null)
        {
            Debugger.LogConsole("No Camera from Cache", 1);
            return;
        }

        // Switching/Activating Camera
        if (activeCamera != null)
        {
            activeCamera.DeactivateCamera();
        }
        activeCamera = camera;
        activeCamera.ActivateCamera();

        // Gather Data from SceneView
        currentCameraData = activeCamera.CameraData;
        currentViewType = currentCameraData.CameraViewType;

        // Switches the current camera view
        SwitchView();
    }

    #endregion

    #region View Activation

    private void SwitchView()
    {
        if (activeCamera == null) return;
        Debugger.LogConsole($"Switching to Camera Type {currentViewType}", 0);

        // camera view logic
        // change transform
        switch (currentViewType)
        {
            case ViewType.VIEW_FOLLOW:
                break;

            case ViewType.VIEW_LOOKAT:
                break;

            case ViewType.VIEW_STATIC: 
                break;

            case ViewType.VIEW_DOLLY: 
                break;
        }
    }

    #endregion
}