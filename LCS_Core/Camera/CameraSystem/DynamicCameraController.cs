using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DynamicCameraController : MonoBehaviour
{
    [SerializeField] private float smoothTime = 1f;             // time of lerp when moving camera

    // Transforms
    private Transform targetTransform = null;
    private Transform sceneCameraTransform = null;

    // Positioning
    private Vector3 targetPosition;
    private float lookAheadOffset = 0.5f;      // offset in the z-dir

    // SceneCamera
    private SceneCamera activeSceneCamera = null;
    private ViewType currentViewType;

    // Dynamics
    private Vector3 dynamicLookOffset = new Vector3(0, 0, 0);   // used to adjust the lookAheadOffset
    private bool useRuntime = false;

    #region Activation

    // Called on Startup
    public void SetTarget(Transform main_target)
    {
        if (main_target == null) return;
        targetTransform = main_target;
    }

    // Called when a new SceneCamera is activated
    public void ViewChanged(ViewType type, SceneCamera camera)
    {
        if (camera == null) return;
        currentViewType = type;
        activeSceneCamera = camera;
        sceneCameraTransform = activeSceneCamera.TargetTransform;
        UpdateCameraBehaviour();
    }

    private void UpdateCameraBehaviour()
    {
        useRuntime = true;
        switch (currentViewType)
        {
            case ViewType.VIEW_STATIC:
                useRuntime = false;
                StaticCamera();
                break;

            case ViewType.VIEW_TRACKING:
                StaticCamera();         // use static for inialising camera position/rotation
                break;

            case ViewType.VIEW_FOLLOW:
                break;
        }
    }

    #endregion

    #region Runtime

    private void Update()
    {
        // Runtime Check
        if (!useRuntime) return;

        if (currentViewType == ViewType.VIEW_TRACKING)
        {
            TrackingCamera();
            return;
        }

        else if (currentViewType == ViewType.VIEW_FOLLOW)
        {
            return;
        }
    }

    #endregion

    #region Static Camera

    private void StaticCamera()
    {
        transform.position = sceneCameraTransform.position;
        transform.rotation = sceneCameraTransform.rotation;
    }

    #endregion

    #region Lookat Camera

    private void TrackingCamera()
    {
        // Initial Offset (preset)
        Vector3 fixedOffset = targetTransform.forward * lookAheadOffset;
        // Add dynamic offset to value
        Vector3 rotatedDynamicOffset = targetTransform.rotation * dynamicLookOffset;
        Vector3 combinedOffset = fixedOffset + rotatedDynamicOffset;
        // Set Target Position and track its position
        targetPosition = targetTransform.position + combinedOffset;
        transform.LookAt(targetPosition);
    }



    #endregion

    #region Debug

    private void OnDrawGizmos()
    {
        if (targetPosition == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(targetPosition, 0.05f);
    }

    #endregion
}