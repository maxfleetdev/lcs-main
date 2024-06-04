using NaughtyAttributes;
using UnityEngine;

public class SceneCamera : MonoBehaviour, ISceneCamera
{
    [SerializeField] private CameraData cameraData;
    [SerializeField] private BoxTrigger trigger;
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
        if (cameraID == -1)
        {
            Debugger.LogConsole("Camera not Initialised", 0, this);
            return;
        }

        if (cameraData == null)
        {
            Debugger.LogConsole("No CameraData Detected", 0, this);
            return;
        }
        trigger.OnTriggerCalled += EnteredCamera;
    }

    private void OnDisable()
    {
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
            CameraHandler.ActivateCamera(cameraID);
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
}