using UnityEngine;

/// <summary>
/// Main Controller for handling the camera in-game. Can switch on the fly from Static to lookat
/// </summary>
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    #region OLD
/*
[Header("Dynamic Camera")]
[SerializeField, Tag] private string targetTag;
[SerializeField] private LayerMask obstacleLayer;
[SerializeField] private float positionSpeed = 0.125f;
[SerializeField] private float rotationSpeed = 0.125f;
[SerializeField] private float collisionRadius = 0.5f;
[SerializeField] private float lookAheadFactor = 1.5f;

[Header("Input")]
[SerializeField] private InputData inputData;

// Transforms
private Transform cameraTarget = null;
private Transform cameraPosition = null;

// Positioning
private Vector3 targetOffset = new Vector3(0, 0, 0.5f);
private Vector3 followOffset = new Vector3(0, 3f, -3f);
private Vector3 targetPosition;

// From SceneCamera
private ViewType currentType;

private float targetLookAheadFactor;

// For dynamically updating view
private bool updateCamera = false;
private bool targetVisible = false;

#region Runtime

private void Update()
{
GetCurrentLookat();

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

private void ChangeFacingDirection()
{
targetLookAheadFactor = 1.5f;
}

private void ReturnFacingDirection()
{
targetLookAheadFactor = 0f;
}

private void GetCurrentLookat()
{
Vector3 direction = cameraTarget.TransformDirection(Vector3.forward);
Debug.DrawRay(cameraTarget.position, direction * 10f, Color.blue);
}

private void AdjustCameraRotation()
{
// Calculate target position with look-ahead
Vector3 lookAheadPosition = cameraTarget.position + (cameraTarget.forward * lookAheadFactor);
targetPosition = lookAheadPosition + (cameraTarget.rotation * targetOffset);
transform.LookAt(targetPosition);

// Smoothly adjust LookAhead
lookAheadFactor = Mathf.Lerp(lookAheadFactor, targetLookAheadFactor, rotationSpeed * Time.deltaTime);
if (lookAheadFactor <= 0.01f)
    lookAheadFactor = 0f;
}

private void AdjustCameraPosition()
{
// Desired position with offset
Vector3 desired_pos = cameraTarget.position + followOffset;
Vector3 direction = desired_pos - cameraTarget.position;
float distance = direction.magnitude;

// Check for obstacles
Ray detect_ray = new Ray(cameraTarget.position, direction);
RaycastHit hit;
if (Physics.Raycast(detect_ray, out hit, distance, obstacleLayer))
{
    Vector3 adjusted_pos = hit.point - direction.normalized * collisionRadius;
    transform.position = Vector3.Lerp(transform.position, adjusted_pos, positionSpeed * Time.deltaTime);
    targetVisible = false;
}
else
{
    transform.position = Vector3.Lerp(transform.position, desired_pos, positionSpeed * Time.deltaTime);
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
*/
#endregion
}