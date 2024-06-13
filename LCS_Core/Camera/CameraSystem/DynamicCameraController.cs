using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DynamicCameraController : MonoBehaviour
{
    [SerializeField] private float smoothTime = 1f;             // time of lerp when moving camera
    [SerializeField] private float pathMoveSpeed = 3f;
    [SerializeField] private LayerMask obstacleLayer;

    // Transforms
    private Transform targetTransform = null;
    private Transform sceneCameraTransform = null;

    // Positioning
    private Vector3 targetPosition;
    private float lookAheadOffset = 0.5f;      // offset in the z-dir

    // SceneCamera
    private SceneCamera activeSceneCamera = null;
    private ViewType viewType;

    // Dynamics
    private Vector3 dynamicLookOffset = new Vector3(0, 0, 0);   // used to adjust the lookAheadOffset
    private bool useRuntime = false;
    private Vector3 velocity;
    
    // CameraData
    private bool isTracking = false;
    private CameraPath currentPath = null;
    private CameraData currentData = null;
    private List<Vector3> currentPoints = new List<Vector3>();

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
        // Data
        currentData = camera.CameraData;
        viewType = currentData.CameraViewType;
        isTracking = currentData.TrackTarget;
        // Camera
        activeSceneCamera = camera;

        // Pathing
        currentPath = activeSceneCamera.GetPath();
        currentPoints.Clear();
        foreach (var p in currentPath.GetNodes())
        {
            currentPoints.Add(p.NodePosition.position);
        }
        sceneCameraTransform = activeSceneCamera.TargetTransform;

        UpdateCameraBehaviour();
    }

    private void UpdateCameraBehaviour()
    {
        useRuntime = true;
        switch (viewType)
        {
            case ViewType.VIEW_STATIC:
                useRuntime = false;
                StaticCamera();
                break;

            case ViewType.VIEW_PATH:
                break;
        }
    }

    #endregion

    #region Runtime

    private void Update()
    {
        if (isTracking) TrackingCamera();
        if (!useRuntime) return;

        if (viewType == ViewType.VIEW_PATH)
        {
            FollowPath();
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

    #region Camera Tracking

    private void TrackingCamera()
    {
        Vector3 fixedOffset = targetTransform.forward * lookAheadOffset;
        Vector3 rotatedDynamicOffset = targetTransform.rotation * dynamicLookOffset;
        Vector3 combinedOffset = fixedOffset + rotatedDynamicOffset;
        targetPosition = targetTransform.position + combinedOffset;
        transform.LookAt(targetPosition);
    }

    #endregion

    #region Path Tracking

    // todo:
    // raycast check for visability, otherwise multiply speed and catch up
    // make sure camera finishes node before skipping to another
    // add offset to camera (distance/dynamic offset)
    private Vector3 closePoint;
    private int pointIndex;

    private void FollowPath()
    {
        PathUtils.FindClosestLineSegment(currentPoints.ToArray(), targetTransform.position, out pointIndex, out closePoint);
        transform.position = Vector3.SmoothDamp(transform.position, closePoint, ref velocity, smoothTime);
    }

    private bool CheckVisability(Vector3 target)
    {
        Vector3 dir = target - transform.position;
        if (Physics.Raycast(transform.position, dir, out RaycastHit hit, Mathf.Infinity))
        {
            if (hit.transform.gameObject.layer == obstacleLayer)
            {
                Debug.DrawRay(transform.position, dir, Color.red);
                print(hit.transform.name);
                return false;
            }
        }
        Debug.DrawRay(transform.position, dir, Color.green);
        return true;
    }

    #endregion

    #region Debug

    private void OnDrawGizmos()
    {
        if (targetTransform == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(targetPosition, 0.05f);

        //Gizmos.color = Color.blue;
        //Gizmos.DrawLine(transform.position, targetTransform.position);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(closePoint, 0.05f);
    }

    #endregion
}