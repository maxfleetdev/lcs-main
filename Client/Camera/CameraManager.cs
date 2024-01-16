using System;
using UnityEngine;
using Cinemachine;

public class CameraManager : MonoBehaviour
{
    // Class-only //
    private static CameraManager instance;
    protected private CinemachineVirtualCamera[] cameras;

    // Public //
    public static CameraManager Instance
    {
        get => instance;
        private set => instance = value;
    }
    public Action<CinemachineVirtualCamera> OnCamChange;

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(this);
            return;
        }

        OnCamChange += CameraChange;
    }

    private void OnDisable()
    {
        if (instance != null)
        {
            instance = null;
        }

        OnCamChange -= CameraChange;
    }

    private void CameraChange(CinemachineVirtualCamera cam)
    {
        DebugSystem.Log("Got camera, changing...", LogType.Debug);
        cam.enabled = true;
    }
}