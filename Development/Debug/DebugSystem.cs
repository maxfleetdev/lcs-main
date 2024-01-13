using UnityEngine;

public enum LogType
{
    Info,
    Debug,
    Warn,
    Error
}

/// <summary>
/// The main class which uses custom made debugging functions for testing and editing purposes.
/// </summary>
public static class DebugSystem
{
    /// <summary>
    /// Sends a message to the console of varying importance.
    /// </summary>
    /// <param name="message"></param>
    /// <param name="type"></param>
    /// <param name="source"></param>
    public static void Log(string message, LogType type, MonoBehaviour source = null)
    {
        // Setting the text color within console
        string color = string.Empty;
        switch (type)
        {
            case LogType.Info:
                color = "white";
                break;
            case LogType.Debug:
                color = "gray";
                break;
            case LogType.Warn:
                color = "yellow";
                break;
            case LogType.Error:
                color = "red";
                break;
            default:
                break;
        }

        // Format final message to send to console
        string log_to_send;
        if (source == null)
            log_to_send = $"{message}";
        else
            log_to_send = $"{source.name}@{message}";

        // Send message to console
        switch (type)
        {
            case LogType.Info:
                Debug.Log(log_to_send);
                break;
            case LogType.Debug:
                Debug.Log(log_to_send);
                break;
            case LogType.Warn:
                Debug.LogWarning(log_to_send);
                break;
            case LogType.Error:
                Debug.LogError(log_to_send);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Clears the console within the editor and in-game.
    /// </summary>
    public static void ClearConsole()
    {
        // Used to clear console in editor and in-game
        Debug.ClearDeveloperConsole();
    }
}