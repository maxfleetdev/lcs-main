using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Dynamic Orbital Camera which moves dynamically with the player in preset camera bounds.
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] private InputData inputData;

    // Active Camera Data
    private SceneCamera activeCamera = null;
    private CameraData currentData = null;
    private CameraBounds currentBound = null;
    private Transform camTarget = null;
    private Transform camTransform;

    // Camera Offsets & Misc
    private Vector3 lookaheadTarget = Vector3.zero;
    private readonly float minDistance = 3f;
    private float aimLength = 1f;
    private readonly float lerpTime = 4f;

    // Camera Bounds
    private Vector3 boundMin;
    private Vector3 boundMax;
    private float[] minBounds;
    private float[] maxBounds;

    // Camera Positioning
    private Vector3 wishPosition;

    #region Start/Stop

    private void OnEnable()
    {
        camTransform = this.transform;
        CameraSwitchHandler.OnCameraChanged += CameraChanged;
        inputData.AimStartEvent += AimStart;
        inputData.AimEndEvent += AimEnd;
    }

    private void OnDisable()
    {
        CameraSwitchHandler.OnCameraChanged -= CameraChanged;
        inputData.AimStartEvent -= AimStart;
        inputData.AimEndEvent -= AimEnd;
    }

    #endregion

    #region Camera Event

    private void CameraChanged(int id)
    {
        activeCamera = CameraCache.GetCamera(id);
        if (activeCamera == null)
        {
            return;
        }
        currentData = activeCamera.GetData();
        currentBound = activeCamera.GetBounds();
        
        // Targetting
        camTarget = PlayerCache.PlayerTransform;
        SetConstraints();

        // Set Static
        if (currentData.CameraViewType == ViewType.VIEW_STATIC)
        {
            StaticCamera();
        }
    }

    // Must be called before applying camera
    private void SetConstraints()
    {
        // Cache New Bounds
        Vector3 center = currentBound.GetCenter();
        Vector3 size = currentBound.GetSize();
        boundMin = currentBound.transform.TransformPoint(center - size / 2);
        boundMax = currentBound.transform.TransformPoint(center + size / 2);
        minBounds = new float[] { boundMin.x, boundMin.y, boundMin.z };
        maxBounds = new float[] { boundMax.x, boundMax.y, boundMax.z };
        aiming = false;
    }

    #endregion

    #region Input

    bool aiming = false;
    private void AimStart()
    {
        if (!currentData.AllowLookahead || aiming) return;
        aiming = true;
        aimLength = 6f;
    }

    private void AimEnd()
    {
        aiming = false;
        aimLength = 1f;
    }

    #endregion

    #region Transition

    #endregion

    #region Runtime

    private void LateUpdate()
    {
        if (currentBound != null || camTarget != null)
        {
            if (currentData.CameraViewType == ViewType.VIEW_STATIC)
            {
                return;
            }
            ConstrainCamera();
            LookatOffset();
        }
    }

    // How LookOffset works:
        // > Get angle between Camera, Player and LookOffset
        // > Invert angle, so 0 degrees = 0, 180 degrees = 1, then use as multiplier for height (& length)
        // > When facing camera, height increase, no more camera spinning around
        // > Then, simply rotate towards the resting point
        // > Moves back to 0 degrees eventually
    private void LookatOffset()
    {
        // Dynamic Height
        float invert_angle = 1f - (FindCameraAngle() / 180);
        float height = (camTransform.position.y - camTarget.position.y) * (invert_angle * 0.75f);

        // Dynamic Length
        float length = Mathf.Abs(Vector3.Distance(camTransform.position, camTarget.position)) / 4;

        // Final Rotation
        lookaheadTarget = camTarget.position + ((camTarget.forward * length * aimLength) + (camTarget.up * height));
        Quaternion target_rotation = Quaternion.LookRotation(lookaheadTarget - camTransform.position);
        camTransform.rotation = Quaternion.Lerp(camTransform.rotation, target_rotation, lerpTime * Time.deltaTime);
    }

    // Just constrains the camera to a set boundary
    private void ConstrainCamera()
    {
        // Get best position
        Vector3 desired_position = GetIdealPosition();

        // Constrain the position to the bounds
        Vector3 constrained_pos = new Vector3(
            Mathf.Clamp(desired_position.x, minBounds[0], maxBounds[0]),
            Mathf.Clamp(desired_position.y, minBounds[1], maxBounds[1]),
            Mathf.Clamp(desired_position.z, minBounds[2], maxBounds[2])
        );

        // Final Positioning
        if (Vector3.Distance(constrained_pos, camTransform.position) < 0.1f)
            return;

        camTransform.position = Vector3.Lerp(camTransform.position, constrained_pos, 4 * Time.deltaTime);
    }

    // Find the best position for the camera before constraining
    private Vector3 GetIdealPosition()
    {
        float distance = FindCameraDistance();

        // Adjust Forward
        float z_adj = 0.5f / (distance + 1);
        Vector3 z_pos = camTransform.forward * (z_adj + minDistance);
        
        // Adjust Height (By Distance)
        float y_adj = 1 / (distance + 1);
        y_adj = Mathf.Clamp(y_adj, 0, 1);
        Vector3 y_pos = Vector3.up * y_adj;

        // Set Final Postion
        Vector3 ideal_pos = camTarget.position - z_pos + y_pos;
        wishPosition = ideal_pos;
        return ideal_pos;
    }

    // needs to be changed w/ cameradata
    private void StaticCamera()
    {
        camTransform.LookAt(activeCamera.GetLookat());
        camTransform.position = currentBound.transform.position;
    }

    #endregion

    #region Utils

    // Finds the angle between the Camera, Player and Lookat positions (Vector2's)
    private float FindCameraAngle()
    {
        Vector2 BA, BC, player, camera, lookat;
        float dot, rads, angle;

        // Use Vector2's to find 2D angle, instead of 3D space
        player = new Vector2(camTarget.position.x, camTarget.position.z);
        camera = new Vector2(transform.position.x, transform.position.z);
        lookat = new Vector2(lookaheadTarget.x, lookaheadTarget.z);

        BA = camera - player;
        BC = lookat - player;

        BA.Normalize();
        BC.Normalize();

        // Convert from radians to degrees
        dot = Vector2.Dot(BA, BC);
        rads = Mathf.Acos(dot);
        angle = rads * Mathf.Rad2Deg;

        return angle;
    }

    private float FindCameraDistance()
    {
        float cx, cz, tx, tz;
        // Assign Vector2
        cx = camTransform.position.x;
        cz = camTransform.position.z;
        tx = camTarget.position.x;      // Maybe change to LookOffsetPosition
        tz = camTarget.position.z;
        // Find Distance
        Vector2 cam = new Vector2(cx, cz);
        Vector2 target = new Vector2(tx, tz);
        return Vector2.Distance(cam, target);
    }

    #endregion

    #region Debug

    private void OnDrawGizmos()
    {
        if (camTarget == null) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(lookaheadTarget, new Vector3(0.075f, 0.075f, 0.075f));
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(wishPosition, 0.08f);
        Gizmos.DrawLine(transform.position, wishPosition);
    }

    #endregion
}