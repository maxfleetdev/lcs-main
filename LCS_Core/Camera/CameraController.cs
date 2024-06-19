using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Dynamic Orbital Camera which moves dynamically with the player in preset camera bounds.
/// </summary>
public class CameraController : MonoBehaviour
{
    [SerializeField] private InputData inputData;
    [SerializeField] private float heightChange;
    [SerializeField] private float horizontalChange;

    // Active Camera Data
    private SceneCamera activeCamera = null;
    private CameraData currentData = null;
    private CameraBounds currentBound = null;
    private Transform target = null;
    private Transform camTransform;

    // Camera Offsets & Misc
    private Vector3 lookaheadTarget = Vector3.zero;
    private readonly float minDistance = 3f;
    private float aimLength = 1f;
    private readonly float lerpTime = 2f;

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
        target = PlayerCache.PlayerTransform;
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

    #region Runtime

    private void LateUpdate()
    {
        if (currentBound != null || target != null)
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
        float angle = 1f - (FindCameraAngle() / 180);
        float height = (camTransform.position.y - target.position.y) * (angle * 0.6f);
        float length = Mathf.Abs(Vector3.Distance(camTransform.position, target.position)) / 4;     // still needs changing
        lookaheadTarget = target.position + ((target.forward * length * aimLength) + (target.up * height));
        Quaternion target_rotation = Quaternion.LookRotation(lookaheadTarget - camTransform.position);
        camTransform.rotation = Quaternion.Lerp(camTransform.rotation, target_rotation, lerpTime * Time.deltaTime);
    }

    // Just constrains the camera to a set boundary
    // TODO: Simplify & more effecient
    private void ConstrainCamera()
    {
        // Get best position
        Vector3 desired_position = GetIdealPosition();
        Vector3 constrained_pos = desired_position;

        // Define Arrays
        float[] cam_pos_a = { desired_position.x, desired_position.y, desired_position.z };
        float[] constrained_pos_a = { constrained_pos.x, constrained_pos.y, constrained_pos.z };

        // Loop through each axis
        for (int i = 0; i < 3; i++)
        {
            if (cam_pos_a[i] < minBounds[i]) 
                constrained_pos_a[i] = minBounds[i];
            if (cam_pos_a[i] > maxBounds[i]) 
                constrained_pos_a[i] = maxBounds[i];
        }
        // Actually bound position 
        constrained_pos.x = constrained_pos_a[0];
        constrained_pos.y = constrained_pos_a[1];
        constrained_pos.z = constrained_pos_a[2];

        // Final Positioning
        if (Vector3.Distance(constrained_pos, camTransform.position) < 0.1f)
            return;
        camTransform.position = Vector3.Lerp(camTransform.position, constrained_pos, 4 * Time.deltaTime);
    }

    // Find the best position for the camera before constraining
    // TODO: Add height adjustment, make higher when player is too close
    private Vector3 GetIdealPosition()
    {
        Vector3 horizontal = camTransform.forward * minDistance;
        Vector3 ideal_pos = target.position - horizontal;
        wishPosition = ideal_pos;
        return ideal_pos;
    }

    private void StaticCamera()
    {
        camTransform.LookAt(activeCamera.GetLookat());
        camTransform.position = currentBound.transform.position;
    }

    // Finds the angle between the Camera, Player and Lookat positions (Vector2's)
    private float FindCameraAngle()
    {
        Vector2 BA, BC, player, camera, lookat;
        float dot, rads, angle;

        // Use Vector2's to find 2D angle, instead of 3D space
        player = new Vector2(target.position.x, target.position.z);
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

    #endregion

    #region Debug

    private void OnDrawGizmos()
    {
        if (target == null) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(lookaheadTarget, new Vector3(0.075f, 0.075f, 0.075f));
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(wishPosition, 0.08f);
        Gizmos.DrawLine(transform.position, wishPosition);
    }

    #endregion
}