/// <summary>
/// Used to find all instances within the LCS codebase
/// </summary>
public static class InstanceFinder
{
    /// <summary>
    /// Returns the instanced Input Manager
    /// </summary>
    /// <returns></returns>
    public static InputManager Input_Manager()
    {
        if (InputManager.Instance != null)
        {
            return InputManager.Instance;
        }
        else
        {
            DebugSystem.Log("InputManger not found! Returning null...", LogType.Error);
            return null;
        }
    }

    /// <summary>
    /// Returns the instanced Debug Console
    /// </summary>
    /// <returns></returns>
    public static DebugConsole Debug_Console()
    {
        if (DebugConsole.Instance != null)
        {
            return DebugConsole.Instance;
        }
        else
        {
            DebugSystem.Log("DebugConsole not found! Returning null...", LogType.Error);
            return null;
        }
    }

    public static PlayerMovement Player_Movement()
    {
        if (PlayerMovement.Instance != null)
        {
            return PlayerMovement.Instance;
        }

        else
        {
            DebugSystem.Log("PlayerMovement not found! Returning null...", LogType.Error);
            return null;
        }
    }

    public static MainCameraController Camera_Controller()
    {
        if (MainCameraController.Instance != null)
        {
            return MainCameraController.Instance;
        }

        else
        {
            DebugSystem.Log("CameraController not found! Returning null...", LogType.Error);
            return null;
        }
    }

    public static CameraManager Camera_Manager()
    {
        if (CameraManager.Instance != null)
        {
            return CameraManager.Instance;
        }

        else
        {
            DebugSystem.Log("CameraManager not found! Returning null...", LogType.Error);
            return null;
        }
    }
}