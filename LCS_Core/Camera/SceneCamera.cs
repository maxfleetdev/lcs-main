using Cinemachine;
using NaughtyAttributes;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class SceneCamera : MonoBehaviour, ISceneCamera
{
    [SerializeField] private CameraData cameraData;
    [SerializeField] private BoxTrigger trigger;
    // For Debugging only
    [ShowNonSerializedField] private int cameraID = -1;
    [ShowNonSerializedField] private bool cameraActive;

    private CinemachineVirtualCamera vcamera;

    #region Startup

    private void Start()
    {
        vcamera = GetComponent<CinemachineVirtualCamera>();
        ToggleVirtualCamera(false);
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
        ToggleVirtualCamera(true);
    }

    public void DeactivateCamera()
    {
        ToggleVirtualCamera(false);
    }

    private void ToggleVirtualCamera(bool toggle)
    {
        vcamera.enabled = toggle;
        cameraActive = toggle;
    }

    #endregion
}