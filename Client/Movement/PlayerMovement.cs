using UnityEngine;

/*
 *  All player movement is calculated here using the InputManager
 *  Player can choose between 1st and 3rd person, changing perspective
 *  and controls.
 */

public class PlayerMovement : MonoBehaviour
{
    // Serialised Vars //
    [Header("Movement")]
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;
    [SerializeField] private float turnSpeed = 90f;

    [Header("Camera")]
    [SerializeField] private Camera firstPersonCam;
    [SerializeField] private float sensitivity = 0.1f;

    // Class-only //
    private static PlayerMovement instance;
    private InputManager inputManager;

    protected private CharacterController charController;

    private Vector2 moveInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;

    private bool isFirstPerson = true;
    private bool isRunning = false;

    private float rotationX;

    // Public Properties //
    public bool IsFirstPerson
    {
        get => isFirstPerson;
        private set => isFirstPerson = value;
    }
    public static PlayerMovement Instance
    {
        get => instance;
        private set => instance = value;
    }

    #region Setup

    private void Start()
    {
        if (InstanceFinder.Input_Manager())
        {
            inputManager = InstanceFinder.Input_Manager();
        }
        if (instance == null)
        {
            instance = this;
        }
        charController = GetComponent<CharacterController>();

        SetupEvents();
        ChangeView(isFirstPerson);
    }

    private void OnDisable()
    {
        if (instance != null)
        {
            instance = null;
        }

        DisableEvents();
    }

    private void SetupEvents()
    {
        inputManager.OnMoving += GetMoveInput;      // move input event
        inputManager.OnMoveEnd += GetMoveInput;

        inputManager.OnLooking += GetLookInput;     // look input event
        inputManager.OnLookEnd += GetLookInput;

        inputManager.OnRunStart += GetRunning;
        inputManager.OnRunEnd += GetRunning;
    }

    private void DisableEvents()
    {
        inputManager.OnMoving -= GetMoveInput;      // move input event
        inputManager.OnMoveEnd -= GetMoveInput;

        inputManager.OnLooking -= GetLookInput;     // look input event
        inputManager.OnLookEnd -= GetLookInput;

        inputManager.OnRunStart -= GetRunning;
        inputManager.OnRunEnd -= GetRunning;
    }

    #endregion

    private void Update()
    {
        // Changes Movement based on view //
        if (isFirstPerson)
        {
            FirstPersonMove();
            CameraLook();
        }

        else
        {
            ThirdPersonMove();
        }


        ///////////////////////////////////
        // Move Logic for both 1st & 3rd //
        ///////////////////////////////////
        
        // grounded
        // clamp speed
        // collision
        // etc
    }

    #region Third-Person Logic

    protected private void ThirdPersonMove()
    {
        // basic input //
        float input_x = moveInput.x;
        float input_y = moveInput.y;
        float move_speed;

        // can't run backwards
        if (input_y > 0)
        {
            move_speed = isRunning ? runSpeed : walkSpeed;
        }
        else
        {
            move_speed = walkSpeed;
        }

        // tank controls //
        transform.Rotate(0, input_x * turnSpeed * Time.deltaTime, 0);
        Vector3 movDir = transform.forward * input_y * move_speed;
        charController.Move(movDir * Time.deltaTime);
    }
    
    #endregion

    #region First-Person Logic

    protected private void FirstPersonMove()
    {
        if (!isFirstPerson)
            return;

        // basic input //
        float move_speed = isRunning ? runSpeed : walkSpeed;
        float input_x = moveInput.x;
        float input_y = moveInput.y;

        // fps controls //
        Vector3 movDir = new Vector3(input_x, 0, input_y) * move_speed;
        movDir = transform.TransformDirection(movDir);
        charController.Move(movDir * Time.deltaTime);
    }

    protected private void CameraLook()
    {
        if (!isFirstPerson)
            return;

        // Input //
        float view_x = lookInput.x;
        float view_y = lookInput.y;

        // Camera Look Angles //
        rotationX -= view_y * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -90f, 90f);

        // Update Transform //
        firstPersonCam.transform.localRotation = Quaternion.Euler(rotationX, 0f, 0f);
        transform.rotation *= Quaternion.Euler(0f, view_x * sensitivity, 0f);
    }

    #endregion

    #region View Logic

    public void ChangeView(bool view)
    {
        isFirstPerson = view;

        if (isFirstPerson)
        {
            firstPersonCam.gameObject.SetActive(true);
            InstanceFinder.Camera_Controller().ToggleCamera(false);
        }

        else
        {
            firstPersonCam.gameObject.SetActive(false);
            InstanceFinder.Camera_Controller().ToggleCamera(true);
        }
    }

    #endregion

    #region Input Events

    private void GetMoveInput(Vector2 input)
    {
        moveInput = input;
    }

    private void GetLookInput(Vector2 input)
    {
        lookInput = input;
    }

    private void GetRunning()
    {
        isRunning = !isRunning;
    }

    #endregion
}