using UnityEngine;
using Cinemachine;

public class DynamicCamera : MonoBehaviour
{
    // Class-only //
    protected private CameraManager cameraManager;
    protected private BoxCollider boxCollider;

    private CinemachineVirtualCamera virtualCamera;

    private void Start()
    {
        cameraManager = InstanceFinder.Camera_Manager();
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
    }

    /*
     *  Todo:
     *  - make a dynamic camera system which changes depending on the players
     *  position.
     *  - change usage if player is in first person mode? might not be needed
     */

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            DebugSystem.Log("Changing Camera...", LogType.Debug);
            cameraManager.OnCamChange?.Invoke(virtualCamera);
        }
    }
}