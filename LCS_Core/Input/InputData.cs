using System;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "LCS/Input")]
public class InputData : ScriptableObject, MainControls.IGameplayActions, MainControls.IUIActions
{
    public event Action<Vector2> MoveEvent;
    public event Action<Vector2> LookEvent;
    public event Action InteractEvent;
    public event Action AimStartEvent;
    public event Action AimEndEvent;
    public event Action DebugEvent;
    public event Action SprintStartEvent;
    public event Action SprintEndEvent;
    public event Action FaceDirectionStart;
    public event Action FaceDirectionEnd;

    private MainControls mainControls;

    #region Construct

    public void Construct()
    {
        if (mainControls == null)
        {
            mainControls = new MainControls();
            mainControls.Gameplay.SetCallbacks(this);
        }
        Debugger.LogConsole("Input Initialised", 0);
        SetGameplay();
    }

    public void Remove()
    {
        mainControls?.Gameplay.Disable();
    }

    #endregion

    #region Input Logic

    public void SetGameplay()
    {
        mainControls.Gameplay.Enable();
        mainControls.UI.Disable();
    }

    public void SetUI()
    {
        mainControls.Gameplay.Disable();
        mainControls.UI.Enable();
    }

    #endregion

    #region Gameplay Controls

    public void OnAim(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            AimStartEvent?.Invoke();
        }
        if (context.phase == InputActionPhase.Canceled)
        {
            AimEndEvent?.Invoke();
        }
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            InteractEvent?.Invoke();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        LookEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        MoveEvent?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnDebug(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            DebugEvent?.Invoke();
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            SprintStartEvent?.Invoke();

        if (context.phase == InputActionPhase.Canceled)
            SprintEndEvent?.Invoke();
    }

    public void OnChangeView(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
            FaceDirectionStart?.Invoke();
        if (context.phase == InputActionPhase.Canceled)
            FaceDirectionEnd?.Invoke();
    }

    #endregion

    #region UI Controls

    public void OnCancel(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnClick(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnMiddleClick(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnNavigate(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnPoint(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnRightClick(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    public void OnSubmit(InputAction.CallbackContext context)
    {
        throw new NotImplementedException();
    }

    #endregion
}