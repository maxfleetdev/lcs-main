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
    [ShowNonSerializedField] private int cameraID = -1;
    [ShowNonSerializedField] private bool cameraActive;

    // Public Properties
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
        // Assign to OnTrigger event
        trigger.OnTriggerCalled += EnteredCamera;
        trigger.OnExitCalled += ExitCamera;
    }

    private void OnDisable()
    {
        // Unassign to OnTrigger event
        trigger.OnTriggerCalled -= EnteredCamera;
        trigger.OnExitCalled -= ExitCamera;
    }

    public void InitialiseCamera(int id)
    {
        cameraID = id;
    }

    #endregion

    #region Activation

    // Called when entering trigger volume
    private void EnteredCamera(bool nested)
    {
        CameraSwitchHandler.ActivateCamera(cameraID, nested);
    }

    // Called when exiting trigger volume
    private void ExitCamera(bool nested)
    {
        CameraSwitchHandler.DeactivateCamera(cameraID);
    }

    #endregion
}