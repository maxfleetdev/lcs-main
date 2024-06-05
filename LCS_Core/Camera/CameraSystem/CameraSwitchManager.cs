using UnityEngine;

/// <summary>
/// Used as a manager for switching Cameras, does not actually control movement. Listens to CameraHandler.
/// </summary>
[RequireComponent(typeof(DynamicCameraController))]
public class CameraSwitchManager : MonoBehaviour
{
    // Camera Switching
    private SceneCamera activeCamera = null;
    private CameraData currentCameraData;
    private DynamicCameraController controller = null;
    private ViewType currentViewType;

    #region Startup

    private void OnEnable()
    {
        // Scene Cameras
        CacheAllCameras();

        // Camera Events
        CameraSwitchHandler.OnActivateCamera += ActivateCamera;
        CameraSwitchHandler.OnForceSwitch += RefreshNewView;
    }

    private void OnDisable()
    {
        CameraSwitchHandler.OnActivateCamera -= ActivateCamera;
        CameraSwitchHandler.OnForceSwitch -= RefreshNewView;
    }

    private void CacheAllCameras()
    {
        // Uses cache rather than this class
        SceneCamera[] cameras = FindObjectsByType<SceneCamera>(FindObjectsSortMode.None);
        CameraCache.CacheCameras(cameras);
        
        // Camera Controller
        controller = GetComponent<DynamicCameraController>();
        controller.SetTarget(PlayerCache.PlayerTransform);
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

    #region View Change

    private void SwitchView()
    {
        if (activeCamera == null) return;
        controller.ViewChanged(currentViewType, activeCamera);
    }

    #endregion

    #region Debugging

    private void RefreshNewView(ViewType viewType)
    {
        currentViewType = viewType;
        SwitchView();
    }

    #endregion
}