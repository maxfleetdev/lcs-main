using UnityEngine;

public static class Debugger
{
    #region Console Logging

    /// <summary>
    /// 0: Debug, 1: Warning, 2: Error
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="level"></param>
    public static void LogConsole(string msg, int level)
    {
        switch (level)
        {
            case 0:
                Debug.Log(msg);
                break;
            case 1:
                Debug.LogWarning(msg);
                break;
            case 2:
                Debug.LogError(msg);
                break;

            default:
                Debug.Log($"<color=#ff4d4d>msg</color>");
                break;
        }
    }

    /// <summary>
    /// 0: Debug, 1: Warning, 2: Error. Asserts Source name
    /// </summary>
    /// <param name="msg"></param>
    /// <param name="level"></param>
    public static void LogConsole(string msg, int level, Object context)
    {
        switch (level)
        {
            case 0:
                Debug.Log($"{msg} || {context.name}");
                break;
            case 1:
                Debug.LogWarning($"{msg} || {context.name}");
                break;
            case 2:
                Debug.LogError($"<color=#ff4d4d>{msg} || {context.name}</color>");
                break;

            default:
                Debug.Log($"{msg} || {context.name}");
                break;
        }
    }

    #endregion
}