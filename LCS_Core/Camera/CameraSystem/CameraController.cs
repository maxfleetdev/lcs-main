using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [SerializeField] private InputData inputData;
    [SerializeField] private LayerMask obstacleLayer;

    // Camera Setup
    private SceneCamera currentCamera = null;
    private CameraData currentData = null;
    private ViewType currentViewType;
    private Transform target;
    private bool useRuntime = false;

    // Tracking
    private Vector3 lookOffset = Vector3.zero;
    private readonly float lookAheadAmount = 0.5f;
    private Vector3 lookatPosition;

    // Spline
    private BezierSpline currentSpline;

    #region Startup and Calls

    private void OnEnable()
    {
        inputData.FaceDirectionStart += ChangeLookStart;        // initial event
        inputData.FaceDirectionEnd += ChangeLookEnd;
        inputData.LookEvent += ChangeLookVector;                    // vector2 coords
    }

    private void OnDisable()
    {
        inputData.FaceDirectionStart -= ChangeLookStart;
        inputData.FaceDirectionEnd -= ChangeLookEnd;
        inputData.LookEvent -= ChangeLookVector;
    }

    // Called when a new SceneCamera is activated
    public void UpdateCameraBehaviour(ViewType type, SceneCamera scene_cam)
    {
        currentCamera = scene_cam;
        currentData = scene_cam.CameraData;
        currentViewType = type;
        target = PlayerCache.PlayerTransform;
        useRuntime = true;
        switch (currentViewType)
        {
            case ViewType.VIEW_STATIC:
                StaticCamera();
                break;

            case ViewType.VIEW_SPLINE:
                currentSpline = currentCamera.GetPath();
                SplinePathFollow();
                break;
        }
    }

    #endregion

    #region Runtime

    private void Update()
    {
        if (!useRuntime || currentCamera == null) return;

        CameraTracking();
        SplinePathFollow();
    }

    #endregion

    #region Input

    private bool changeLookDir = false;
    private void ChangeLookStart()
    {
        changeLookDir = true;
        ChangeLookVector(Vector2.zero);
    }

    private void ChangeLookEnd()
    {
        changeLookDir = false;
        lookOffset = Vector3.zero;
    }

    private void ChangeLookVector(Vector2 dir)
    {
        if (!changeLookDir || !currentData.AllowLookahead) return;
        Vector3 hitPoint = GetViewPoint();
        float zOffset = hitPoint != Vector3.zero ? hitPoint.z : 2f;
        lookOffset = new Vector3(dir.x * 2, dir.y * 2, -zOffset);       // change to be more dynamic
    }

    private Vector3 GetViewPoint()
    {
        RaycastHit hit;
        Vector3 dir = target.forward;
        if (Physics.Raycast(target.position, dir, out hit, Mathf.Infinity, obstacleLayer))
        {
            return hit.point;
        }
        else
        {
            return Vector3.zero;
        }
    }



    #endregion

    #region Tracking

    private readonly float rotationSmoothTime = 0.075f;
    private Quaternion targetRotation;
    private void CameraTracking()
    {
        if (!currentData.TrackTarget) return;
        
        Vector3 fixedOffset = target.forward * lookAheadAmount;
        Vector3 rotatedDynamicOffset = target.rotation * lookOffset;
        Vector3 combinedOffset = fixedOffset + rotatedDynamicOffset;
        lookatPosition = target.position + combinedOffset;
        targetRotation = Quaternion.LookRotation(lookatPosition - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime / rotationSmoothTime);
    }

    #endregion

    #region Positions

    // Used for stationary angles
    private void StaticCamera()
    {
        transform.SetPositionAndRotation(currentCamera.transform.position, currentCamera.transform.rotation);
        useRuntime = currentData.TrackTarget;
    }

    // Follows a set spline path
    private Vector3 velocity;
    private void SplinePathFollow()
    {
        if (currentViewType != ViewType.VIEW_SPLINE || currentSpline == null) return;

        // Calculate position on Spline & apply offset
        float closestT = currentSpline.GetClosestPointParameter(target.position);
        Vector3 targetPosOnSpline = currentSpline.GetPoint(closestT);
        Vector3 splineDirection = currentSpline.GetDirection(closestT).normalized;
        float offsetDistance = 2.0f;
        Vector3 offset = splineDirection * offsetDistance;
        Vector3 targetCameraPosition = targetPosOnSpline + offset;
        Vector3 clampedPosition = ConstrainToSpline(targetCameraPosition);

        // Final Movement
        transform.position = Vector3.SmoothDamp(transform.position, clampedPosition, ref velocity, 1f);
    }

    private Vector3 ConstrainToSpline(Vector3 targetPosition)
    {
        Vector3 closestPoint = currentSpline.GetClosestPoint(targetPosition);
        return closestPoint;
    }




    #endregion

    #region Debug

    private void OnDrawGizmos()
    {
        // Line -> Lookat Point
        if (currentData != null && currentData.TrackTarget)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(lookatPosition, 0.07f);
            Gizmos.color = Color.blue;
            //Gizmos.DrawLine(transform.position, lookatPosition);
        }
    }

    #endregion
}