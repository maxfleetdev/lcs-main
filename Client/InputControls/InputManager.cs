using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    #region Variables

    // Class Only //
    protected static private InputManager instance;

    // Private //
    private MainControls mainControls;

    private InputAction cMove;
    private InputAction cLook;
    private InputAction cFire;
    private InputAction cInteract;
    private InputAction cInventory;
    private InputAction cPause;
    private InputAction cMap;
    private InputAction cRun;
    private InputAction cAim;
    private InputAction cStrafe;
    private InputAction cChange;
    private InputAction cDebugConsole;

    // Exposed Properties //
    public static InputManager Instance
    {
        get => instance;
        private set => instance = value;
    }

    public Action<Vector2> OnMoveStart;      // move events
    public Action<Vector2> OnMoving;
    public Action<Vector2> OnMoveEnd;

    public Action<Vector2> OnLookStart;      // look events
    public Action<Vector2> OnLooking;
    public Action<Vector2> OnLookEnd;

    public Action OnFire;              // event triggers
    public Action OnInteract;
    public Action OnInventory;
    public Action OnPause;
    public Action OnMap;

    public Action OnRunStart;
    public Action OnRunEnd;

    public Action OnAim;
    public Action OnDebugConsole;

    public Action<float> OnStrafe;           // pos/neg value
    public Action<float> OnChange;

    #endregion

    #region Startup/Shutdown

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }

        else if (InstanceFinder.Input_Manager())
        {
            // Instance already created so destroy
            DebugSystem.Log("Instance found! Destroying...", LogType.Error, this);
            Destroy(gameObject);
        }

        if (mainControls == null)
        {
            mainControls = new MainControls();
        }

        StartupManager();
        DebugSystem.Log("Input Manager Started...", LogType.Debug);
    }

    private void OnDisable()
    {
        DisableManager();
        instance = null;        // clear instance
        DebugSystem.Log("Input Manager Destroyed...", LogType.Debug);
    }

    #endregion

    protected private void StartupManager()
    {
        // define inputs
        cMove = mainControls.Player.Move;
        cLook = mainControls.Player.Look;
        cFire = mainControls.Player.Fire;
        cInteract = mainControls.Player.Interact;
        cInventory = mainControls.Player.Inventory;
        cPause = mainControls.Player.Pause;
        cMap = mainControls.Player.Map;
        cRun = mainControls.Player.Run;
        cAim = mainControls.Player.Aim;
        cStrafe = mainControls.Player.Strafe;
        cChange = mainControls.Player.Change;
        cDebugConsole = mainControls.Player.DebugConsole;

        // enable inputs
        cMove.Enable();
        cLook.Enable();
        cFire.Enable();
        cInteract.Enable();
        cInventory.Enable();
        cPause.Enable();
        cMap.Enable();
        cRun.Enable();
        cAim.Enable();
        cStrafe.Enable();
        cChange.Enable();
        cDebugConsole.Enable();

        // define events
        cMove.started += ctx => OnMoveStart?.Invoke(cMove.ReadValue<Vector2>());        // move events
        cMove.performed += ctx => OnMoving?.Invoke(cMove.ReadValue<Vector2>());
        cMove.canceled += ctx => OnMoveEnd?.Invoke(cMove.ReadValue<Vector2>());

        cLook.started += ctx => OnLookStart?.Invoke(cLook.ReadValue<Vector2>());        // look events
        cLook.performed += ctx => OnLooking?.Invoke(cLook.ReadValue<Vector2>());
        cLook.canceled += ctx => OnLookEnd?.Invoke(cLook.ReadValue<Vector2>());

        cFire.performed += ctx => OnFire?.Invoke();                                     // action events
        cInteract.performed += ctx => OnInteract?.Invoke();
        cInventory.performed += ctx => OnInventory?.Invoke();
        cPause.performed += ctx => OnPause?.Invoke();
        cMap.performed += ctx => OnMap?.Invoke();

        cRun.started += ctx => OnRunStart?.Invoke();
        cRun.canceled += ctx => OnRunEnd?.Invoke();

        cAim.performed += ctx => OnAim?.Invoke();
        cDebugConsole.performed += ctx => OnDebugConsole?.Invoke();

        cStrafe.performed += ctx => OnStrafe?.Invoke(cStrafe.ReadValue<float>());       // float events (-1, 1)
        cChange.performed += ctx => OnChange?.Invoke(cChange.ReadValue<float>());

        mainControls.Enable();
    }

    protected private void DisableManager()
    {
        // enable inputs
        cMove.Disable();
        cLook.Disable();
        cFire.Disable();
        cInteract.Disable();
        cInventory.Disable();
        cPause.Disable();
        cMap.Disable();
        cRun.Disable();
        cAim.Disable();
        cStrafe.Disable();
        cChange.Disable();
        cDebugConsole.Disable();

        // define events
        cMove.started -= ctx => OnMoveStart?.Invoke(cMove.ReadValue<Vector2>());        // move events
        cMove.performed -= ctx => OnMoving?.Invoke(cMove.ReadValue<Vector2>());
        cMove.canceled -= ctx => OnMoveEnd?.Invoke(cMove.ReadValue<Vector2>());

        cLook.started -= ctx => OnLookStart?.Invoke(cLook.ReadValue<Vector2>());        // look events
        cLook.performed -= ctx => OnLooking?.Invoke(cLook.ReadValue<Vector2>());
        cLook.canceled -= ctx => OnLookEnd?.Invoke(cLook.ReadValue<Vector2>());

        cFire.performed -= ctx => OnFire?.Invoke();                                     // action events
        cInteract.performed -= ctx => OnInteract?.Invoke();
        cInventory.performed -= ctx => OnInventory?.Invoke();
        cPause.performed -= ctx => OnPause?.Invoke();
        cMap.performed -= ctx => OnMap?.Invoke();

        cRun.started -= ctx => OnRunStart?.Invoke();
        cRun.canceled -= ctx => OnRunEnd?.Invoke();

        cAim.performed -= ctx => OnAim?.Invoke();
        cDebugConsole.performed -= ctx => OnDebugConsole?.Invoke();

        cStrafe.performed -= ctx => OnStrafe?.Invoke(cStrafe.ReadValue<float>());       // float events (-1, 1)
        cChange.performed -= ctx => OnChange?.Invoke(cChange.ReadValue<float>());

        mainControls.Disable();
    }
}