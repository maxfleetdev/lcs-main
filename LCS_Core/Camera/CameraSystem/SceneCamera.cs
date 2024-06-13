using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Used as a container for holding data and transforms. Uses BoxTrigger to activate/deactivate this SceneCamera
/// </summary>
public class SceneCamera : MonoBehaviour, ISceneCamera
{
    [SerializeField] private CameraData cameraData;
    [SerializeField] private BoxTrigger trigger;
    [Space]
    // Find bertter way to show this info
    [SerializeField] private bool pathRequired = false;
    [SerializeField, EnableIf("pathRequired")] private CameraPath cameraPath = null;

    // For Inspector
    [Space]
    [ShowNonSerializedField] private int cameraID = -1;
    [ShowNonSerializedField] private bool cameraActive;

    public Transform TargetTransform 
    { 
        get { return transform; }
        private set { } 
    }
    public CameraData CameraData
    {
        get => cameraData;
        private set => cameraData = value;
    }

    #region Startup

    private void Start()
    {
        // Null Checks
        if (cameraID == -1)
        {
            Debugger.LogConsole("Camera not Initialised", 2, this);
            this.enabled = false;
            return;
        }
        if (cameraData == null)
        {
            Debugger.LogConsole("No CameraData Detected", 2, this);
            this.enabled = false;
            return;
        }
        if (cameraData.CameraViewType == ViewType.VIEW_PATH && cameraPath == null)
        {
            Debugger.LogConsole("No CameraPath Detected", 2, this);
            this.enabled = false;
            return;
        }

        // Assign to OnTrigger event
        trigger.OnTriggerCalled += EnteredCamera;
    }

    private void OnDisable()
    {
        // Unassign to OnTrigger event
        trigger.OnTriggerCalled -= EnteredCamera;
    }

    public void InitialiseCamera(int id)
    {
        cameraID = id;
    }

    #endregion

    #region Activation

    private void EnteredCamera()
    {
        if (!cameraActive)
        {
            CameraSwitchHandler.ActivateCamera(cameraID);
        }
    }

    public void ActivateCamera()
    {
        cameraActive = true;
        // activates this SceneCamera to the ViewManager
    }

    public void DeactivateCamera()
    {
        cameraActive = false;
        // deactivates this SceneCamera to the ViewManager
    }

    #endregion

    public CameraPath GetPath() => cameraPath;
}