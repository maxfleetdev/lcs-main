using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private InputData inputData;       // Used for looking

    private SceneCamera activeCamera = null;
    private CameraData currentData = null;
    private CameraBounds currentBound = null;
    private Transform target = null;

    private Vector3 lookaheadTarget = Vector3.zero;
    private readonly float minDistance = 3f;
    private float lookaheadDistance = 0.5f;
    private readonly float moveSmoothTime = 0.25f;
    private readonly float lookSmoothTime = 6f;

    private Vector3 BoundMin;
    private Vector3 BoundMax;
    private float[] MinBounds;
    private float[] MaxBounds;

    #region Start/Stop

    private void OnEnable()
    {
        CameraSwitchHandler.OnCameraChanged += CameraChanged;
        inputData.AimStartEvent += AimDetected;
        inputData.AimEndEvent += AimDetected;
    }

    private void OnDisable()
    {
        CameraSwitchHandler.OnCameraChanged -= CameraChanged;
        inputData.AimStartEvent -= AimDetected;
        inputData.AimEndEvent -= AimDetected;
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
    }

    // Must be called before applying camera
    private void SetConstraints()
    {
        // Cache New Bounds
        Vector3 center = currentBound.GetCenter();
        Vector3 size = currentBound.GetSize();
        BoundMin = currentBound.transform.TransformPoint(center - size / 2);
        BoundMax = currentBound.transform.TransformPoint(center + size / 2);
        MinBounds = new float[] { BoundMin.x, BoundMin.y, BoundMin.z };
        MaxBounds = new float[] { BoundMax.x, BoundMax.y, BoundMax.z };
    }

    #endregion

    #region Input

    bool detected = false;
    private void AimDetected()
    {
        detected = !detected;
        if (detected)
            lookaheadDistance = 6f;
        else
            lookaheadDistance = 0.5f;
    }

    #endregion

    #region Runtime

    private void LookatOffset()
    {
        lookaheadTarget = target.position + ((target.forward * lookaheadDistance) + (target.up * 0.55f));
        Quaternion target_rotation = Quaternion.LookRotation(lookaheadTarget - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, target_rotation, lookSmoothTime * Time.deltaTime);
    }

    private Vector3 velocity;
    private Vector3 wishPosition;
    private void ConstrainCamera()
    {
        // Define Positions
        Vector3 desired_position = target.position - transform.forward * minDistance;
        wishPosition = desired_position;
        Vector3 constrained_pos = desired_position;

        // Define Arrays
        float[] cam_pos_a = { desired_position.x, desired_position.y, desired_position.z };
        float[] constrained_pos_a = { constrained_pos.x, constrained_pos.y, constrained_pos.z };

        // Check distance from player to camera
        float player_dist = Vector3.Distance(transform.position, target.position);
        if (player_dist < minDistance)
        {
            // Calculate upward movement based on camera-to-player direction
            Vector3 move_dir = (transform.position - target.position).normalized;
            constrained_pos += move_dir * (minDistance - player_dist);
        }

        // Loop through each axis
        for (int i = 0; i < 3; i++)
        {
            if (cam_pos_a[i] < MinBounds[i]) 
                constrained_pos_a[i] = MinBounds[i];
            if (cam_pos_a[i] > MaxBounds[i]) 
                constrained_pos_a[i] = MaxBounds[i];
        }
        // Actually bound position 
        constrained_pos.x = constrained_pos_a[0];
        constrained_pos.y = constrained_pos_a[1];
        constrained_pos.z = constrained_pos_a[2];
        
        // Final Position
        transform.position = Vector3.SmoothDamp(transform.position, constrained_pos, ref velocity, moveSmoothTime);
    }

    private void Update()
    {
        if (currentBound != null || target != null)
        {
            LookatOffset();
            ConstrainCamera();
        }
    }

    #endregion

    private void OnDrawGizmos()
    {
        if (target == null) return;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(lookaheadTarget, new Vector3(0.075f, 0.075f, 0.075f));
        Gizmos.color = Color.cyan;
        Gizmos.DrawSphere(wishPosition, 0.08f);
    }
}