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
}