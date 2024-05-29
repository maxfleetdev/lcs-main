using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IDataPersistence
{
    [SerializeField] private InputData inputHandler;
    [SerializeField] private MoveData moveData;
    [SerializeField] private ControllerData controllerData;

    private CharacterController cc;
    private MoveSimulation moveSim;

    private MoveDirection moveDir;

    private Vector3 velocity, prevPosition, movePosition;
    private Vector2 currentInput;

    private bool sprinting = false, grounded = true, strafing = false;
    private float moveSpeed = 0f, currentSpeed;
    private int strafeInput = 0;

    #region Startup

    private void OnEnable()
    {
        SetupController();
    }

    private void OnDisable()
    {
        CleanupController();
    }

    private void SetupController()
    {
        cc = GetComponent<CharacterController>();

        // Input Setup
        inputHandler.MoveEvent += MoveInput;

        inputHandler.SprintStartEvent += SprintInput;
        inputHandler.SprintEndEvent += SprintInput;

        //inputHandler.OnStrafeStart += StrafeInput;
        //inputHandler.OnStrafeEnd += StrafeInput;

        // Move Setup
        moveSim = new MoveSimulation().Startup();
        moveSim.OnTargetAim += SetLookRotation;
        moveSim.OnChangePosition += ExternalSetPosition;
    }

    private void CleanupController()
    {
        // Input Shutdown
        inputHandler.MoveEvent -= MoveInput;
        
        // Reset Variables
        currentInput = Vector2.zero;
        velocity = Vector3.zero;

        // Move Cleanup
        moveSim.OnTargetAim -= SetLookRotation;
        moveSim.OnChangePosition -= ExternalSetPosition;
        moveSim.Cleanup();
    }

    #endregion

    #region Save/Load Logic

    public void LoadData(GameData data)
    {
        Vector3 pos = data.PlayerPositon;
        float rot = data.PlayerRotation;
        LoadPosition(pos, rot);
    }

    public void SaveData(GameData data)
    {
        data.PlayerPositon = this.transform.position;
        data.PlayerRotation = (int)this.transform.eulerAngles.y;
    }

    #endregion

    #region Runtime

    private void Update()
    {
        MovePlayer();
        RotatePlayer();
        CalculateSpeed();
    }

    private void RotatePlayer()
    {
        if (strafing)
            return;

        Vector3 turn = moveSim.UpdateRotation(currentInput, moveData.RotationSpeed);
        transform.Rotate(turn);
    }

    private void MovePlayer()
    {
        moveSpeed = sprinting ? moveData.SprintSpeed : moveData.WalkSpeed;

        // Normal Movement
        Vector3 local_pos = moveSim.UpdatePosition(currentInput, moveSpeed);
        movePosition = transform.TransformDirection(local_pos);

        // Gravity Logic
        CheckGravity();
        movePosition.y = velocity.y;

        // Final Movement
        cc.Move(movePosition);
    }

    private void CheckGravity()
    {
        grounded = cc.isGrounded;
        if (!grounded)
        {
            velocity.y -= moveData.Gravity * Time.deltaTime;
            return;
        }
        velocity.y = -1f * Time.deltaTime;
    }

    private void CalculateSpeed()
    {
        Vector3 current_pos = transform.position;
        currentSpeed = moveSim.GetSpeed(current_pos, prevPosition);
        prevPosition = current_pos;
    }

    /*private bool StrafePlayer()
    {
        // Strafing Movement Only
        if (strafing && currentState == PlayerState.PLAYER_IDLE)
        {
            Vector3 strafe_pos = moveSim.UpdateStrafePosition(strafeInput, moveData.StrafeSpeed);
            movePosition = transform.TransformDirection(strafe_pos);
            CheckGravity();
            movePosition.y = velocity.y;
            cc.Move(movePosition);
            return false;
        }
        return true;
    }*/


    #endregion

    #region Move Events

    // Used class-only
    private void LoadPosition(Vector3 pos, float y_rot)
    {
        cc.enabled = false;
        this.transform.position = pos;
        this.transform.eulerAngles = new Vector3(0, y_rot, 0);
        cc.enabled = true;
    }

    // Other classes control the players position
    private void ExternalSetPosition(Vector3 pos)
    {
        cc.enabled = false;     // disable controller first
        this.transform.position = pos;
        cc.enabled = true;
    }

    private void SetLookRotation(Vector3 pos)
    {
        
    }

    #endregion

    #region Input Events

    private void MoveInput(Vector2 input)
    {
        Vector2 adj_input = input;
        float abs_y = Mathf.Abs(input.y);
        float abs_x = Mathf.Abs(input.x);
        
        // Y Stick Movement
        if (abs_y < controllerData.YSlowMoveDeadzone)
        {
            adj_input.y = 0f;
        }
        else if (abs_y > controllerData.YFastMoveDeadzone)
        {
            adj_input.y = input.y;
        }

        // X Stick Movement
        if (abs_x < controllerData.XSlowMoveDeadzone)
        {
            adj_input.x = 0f;
        }
        else if (abs_x > controllerData.XFastMoveDeadzone)
        {
            adj_input.x = input.x;
        }
        currentInput = adj_input;
    }

    private void SprintInput()
    {
        sprinting = !sprinting;
    }

    private void StrafeInput(float input)
    {
        strafeInput = (int)input;
        strafing = Mathf.Abs(strafeInput) > 0;
    }

    #endregion

    #region Debugging

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        DrawInputLines();
        DrawGroundedSphere();
    }

    private void DrawInputLines()
    {
        // Current Input //
        Vector3 local_dir = new Vector3(currentInput.x, 0, currentInput.y);
        Vector3 world_dir = transform.TransformDirection(local_dir);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(transform.position, transform.position + world_dir);
        
        // Facing Direction //
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward);
        
        // Velocity Direction //
        Gizmos.color = Color.green;
        Vector3 non_y_velocity = new Vector3(velocity.x, 0f, velocity.z);
        Gizmos.DrawLine(transform.position, transform.position + (non_y_velocity.normalized * currentSpeed / 2));
    }

    private void DrawGroundedSphere()
    {
        Vector3 sphere_pos = transform.position + -transform.up;
        Gizmos.color = grounded ? Color.green : Color.red;
        Gizmos.DrawSphere(sphere_pos, 0.15f);
    }

#endif
    #endregion
}

public enum MoveDirection
{
    WALK_FORWARD,
    RUN_FORWARD,
    WALK_BACKWARD,
    RUN_BACKWARD,
    IDLE
}