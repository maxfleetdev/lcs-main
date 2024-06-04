using NaughtyAttributes;
using UnityEngine;

/// <summary>
/// Main Controller for handling the camera in-game. Can switch on the fly from Static to lookat
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    [Header("Dynamic Camera")]
    [SerializeField, Tag] private string targetTag;
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private float smoothSpeed = 0.125f;
    [SerializeField] private float collisionRadius = 0.5f;
    [SerializeField] private float lookAheadFactor = 1.5f;

    [Header("Input")]
    [SerializeField] private InputData inputData;

    // Transforms
    private Transform cameraTarget = null;
    private Transform cameraPosition = null;
    private Transform previousPosition = null;          // just in-case

    // Positioning
    private Vector3 targetOffset = new Vector3(0, 0, 0.5f);     // targets front of player
    private Vector3 followOffset = new Vector3(0, 3f, -4f);     // make dynamic
    private Vector3 targetPosition;

    // From SceneCamera
    private ViewType currentType;

    // For dynamically updating view
    private bool updateCamera = false;
    private bool targetVisible = false;

    #region Runtime

    private void Update()
    {
        // make more effecient and modular
        if (!updateCamera) return;
        AdjustCameraRotation();
        if (currentType == ViewType.VIEW_FOLLOW)
        {
            AdjustCameraPosition();
        }
    }

    #endregion

    #region Activation

    public void SetupController(Transform view_target)
    {
        cameraTarget = view_target;
        inputData.FaceDirectionStart += ChangeFacingDirection;
        inputData.FaceDirectionEnd += ReturnFacingDirection;
    }

    public void SwitchView(ViewType type, Transform position)
    {
        currentType = type;
        cameraPosition = position;
        currentType = type;
        previousPosition = transform;
        switch (currentType)
        {
            case ViewType.VIEW_LOOKAT:
                LookAtView();
                break;

            case ViewType.VIEW_STATIC:
                StaticView();
                break;

            case ViewType.VIEW_FOLLOW:
                FollowView();
                break;
        }
        Debugger.LogConsole($"Camera: {type}, {position}", 0);
    }

    #endregion

    #region View Logic

    private void LookAtView()
    {
        // Setup Transforms for Camera Positioning
        transform.position = cameraPosition.position;
        transform.rotation = cameraPosition.rotation;
        updateCamera = true;
    }

    private void StaticView()
    {
        // Setup Transforms for Camera Positioning
        transform.position = cameraPosition.position;
        transform.rotation = cameraPosition.rotation;
        updateCamera = false;
    }

    private void FollowView()
    {
        updateCamera = true;
    }

    #endregion

    #region Dynamic View

    // Changes current facing direction to the targets forward direction
    private void ChangeFacingDirection()
    {
        Vector3 target_front = cameraTarget.forward;
    }

    // Returns or cancels the direction to default
    private void ReturnFacingDirection()
    {
        
    }

    private void AdjustCameraRotation()
    {

        // Calculate target position with look-ahead
        Vector3 lookAheadPosition = cameraTarget.position + (cameraTarget.forward * lookAheadFactor);
        targetPosition = lookAheadPosition + (cameraTarget.rotation * targetOffset);
        transform.LookAt(targetPosition);
    }

    private void AdjustCameraPosition()
    {
        // Desired position with offset
        Vector3 desiredPosition = cameraTarget.position + followOffset;
        Vector3 direction = desiredPosition - cameraTarget.position;
        float distance = direction.magnitude;

        // Check for obstacles
        Ray detectRay = new Ray(cameraTarget.position, direction);
        RaycastHit hit;

        if (Physics.Raycast(detectRay, out hit, distance, obstacleLayer))
        {
            Vector3 adjustedPosition = hit.point - direction.normalized * collisionRadius;
            transform.position = Vector3.Lerp(transform.position, adjustedPosition, smoothSpeed * Time.deltaTime);
            targetVisible = false;
        }
        else
        {
            // Move to the desired position smoothly
            transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            targetVisible = true;
        }
    }

    #endregion

    #region Debugging

    private void OnDrawGizmos()
    {
        if (cameraTarget == null) return;
        // Target
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(targetPosition, 0.1f);

        // Ray
        Gizmos.color = targetVisible ? Color.green : Color.red;
        Vector3 desiredPosition = cameraTarget.position + followOffset;
        Vector3 direction = desiredPosition - cameraTarget.position;
        float distance = direction.magnitude;
        Gizmos.DrawRay(cameraTarget.position, direction * distance);
    }

    #endregion
}