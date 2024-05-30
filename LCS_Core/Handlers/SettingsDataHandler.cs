using System;

/// <summary>
/// Main Handler for saving and loading SettingsData files. Used in conjunction with SettingsDataFinder.
/// </summary>
public static class SettingsDataHandler
{
    // Requests
    public static event Action OnSettingsSave;
    public static event Action OnSettingsLoad;

    // Replies
    public static event Action OnSettingsSaveComplete;

    #region Requests

    public static void LoadSettings()
    {
        OnSettingsLoad?.Invoke();
    }

    public static void SaveSettings()
    {
        OnSettingsSave?.Invoke();
    }

    #endregion

    #region Replies

    public static void SettingsSaved()
    {
        OnSettingsSaveComplete?.Invoke();
    }

    #endregion
}