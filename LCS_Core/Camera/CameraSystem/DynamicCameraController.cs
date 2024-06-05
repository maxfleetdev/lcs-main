using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DynamicCameraController : MonoBehaviour
{
    [SerializeField] private float lookAheadOffset = 0.5f;      // offset in the z-dir
    [SerializeField] private LayerMask obstacleLayer;           // layer which intersects with camera
    [SerializeField] private float smoothTime = 1f;             // time of lerp when moving
    [SerializeField] private float obstacleLength = 1.5f;

    // Transforms
    private Transform targetTransform = null;
    private Transform sceneCameraTransform = null;

    // Positioning
    private Vector3 targetPosition;
    private Vector3 targetRotation;
    private Vector3 targetOffset;
    [SerializeField] private Vector3 positionOffset = new Vector3(0, 3, -4);     // default values

    // SceneCamera
    private SceneCamera activeSceneCamera = null;
    private ViewType currentViewType;

    // Dynamics
    private float dynamicLookAhead = 1f;        // multiplier z-offset during freelooking
    private bool useRuntime = false;
    private float maxAvoidanceDistance = 2f;
    [SerializeField] private float avoidanceStrength = 2f;       // higher = faster movement

    // Cache
    private Transform cachedTransform;

    #region Activation

    // Called on Startup
    public void SetTarget(Transform main_target)
    {
        if (main_target == null) return;
        targetTransform = main_target;
        cachedTransform = transform;
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

            case ViewType.VIEW_LOOKAT:
                StaticCamera();         // use static for inialising camera position/rotation
                break;

            case ViewType.VIEW_FOLLOW:
                DynamicFollowCamera();
                break;
        }
    }

    #endregion

    #region Runtime

    private void Update()
    {
        // Runtime Check
        if (!useRuntime) return;

        if (currentViewType == ViewType.VIEW_LOOKAT)
        {
            LookatCamera();
            return;
        }

        else if (currentViewType == ViewType.VIEW_FOLLOW)
        {
            DynamicFollowCamera();
            return;
        }
    }

    #endregion

    //////////////////
    // CAMERA LOGIC //
    //////////////////

    #region Static Camera

    private void StaticCamera()
    {
        transform.position = sceneCameraTransform.position;
        transform.rotation = sceneCameraTransform.rotation;
    }

    #endregion

    #region Lookat Camera

    private void LookatCamera()
    {
        // Dynamic Lookahead
        Vector3 lookAheadPosition = targetTransform.position +
            (targetTransform.forward * lookAheadOffset * dynamicLookAhead);

        // Set Positioning
        targetPosition = lookAheadPosition + (targetTransform.rotation * targetOffset);

        // Look directly at Target Position
        transform.LookAt(targetPosition);
    }

    #endregion

    #region Dynamic Camera

    private void DynamicFollowCamera()
    {
        Vector3 moveDir = targetTransform.position + AdjustedOffset();
        moveDir += ObstacleAvoidance() * avoidanceStrength;
        cachedTransform.position = Vector3.Lerp(transform.position, moveDir, smoothTime * Time.deltaTime);
        LookatCamera();
    }

    // Adjusts view offset
    private Vector3 AdjustedOffset()
    {
        if (!IsTargetVisible())
        {
            positionOffset.z = -positionOffset.z;
        }
        return positionOffset;
    }

    // Checks for Target Visibility
    private bool IsTargetVisible()
    {
        Vector3 directionToTarget = targetTransform.position - cachedTransform.position;
        float distanceToTarget = directionToTarget.magnitude;
        Ray ray = new Ray(cachedTransform.position, directionToTarget.normalized);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, distanceToTarget, obstacleLayer))
        {
            return false;
        }
        return true;
    }

    // Moves camera based on surroundings
    private Vector3 ObstacleAvoidance()
    {
        Vector3 avoidanceDir = Vector3.zero;
        Vector3[] directions = {
        cachedTransform.forward, -cachedTransform.forward,
        cachedTransform.right, -cachedTransform.right,
        cachedTransform.up, -cachedTransform.up
    };
        RaycastHit hit;
        foreach (Vector3 dir in directions)
        {
            Ray ray = new Ray(cachedTransform.position, dir);
            if (Physics.Raycast(ray, out hit, obstacleLength, obstacleLayer))
            {
                Debug.DrawLine(ray.origin, hit.point, Color.red);
                float distanceToObstacle = hit.distance;
                float avoidanceStrength = Mathf.Clamp01((maxAvoidanceDistance - distanceToObstacle) / maxAvoidanceDistance);
                avoidanceDir += hit.normal * avoidanceStrength;
            }
        }

        return avoidanceDir;
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