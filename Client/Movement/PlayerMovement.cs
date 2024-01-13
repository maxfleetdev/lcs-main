using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 *  All player movement is calculated here using the InputManager
 *  Player can choose between 1st and 3rd person, changing perspective
 *  and controls.
 */

public class PlayerMovement : MonoBehaviour
{
    // Serialised Vars //
    [SerializeField] private float walkSpeed = 3f;
    [SerializeField] private float runSpeed = 6f;

    // Class-only //
    private PlayerMovement instance;
    private InputManager inputManager;

    protected private CharacterController charController;

    private Vector2 moveInput = Vector2.zero;
    private Vector2 lookInput = Vector2.zero;

    private bool isFirstPerson = false;
    private bool isGrounded = false;
    private bool isRunning = false;

    // Public Properties //
    public bool IsFirstPerson
    {
        get => isFirstPerson;
        private set => isFirstPerson = value;
    }
    public PlayerMovement Instance
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
        // Changes Movement based on choice //
        if (isFirstPerson)
        {
            FirstPersonMove();
        }

        else
        {
            ThirdPersonMove();
        }


        ///////////////////////////////////
        // Move Logic for both 1st & 3rd //
        ///////////////////////////////////

    }

    protected private void ThirdPersonMove()
    {
        float move_speed = isRunning ? runSpeed : walkSpeed;        // check if sprinting
        float input_x = moveInput.x;
        float input_y = moveInput.y;
    }

    protected private void FirstPersonMove()
    {

    }

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