using System;

/// <summary>
/// Main Handler for saving and loading SettingsData files. Used in conjunction with SettingsDataFinder.
/// </summary>
public static class SettingsDataHandler
{
    public static event Action OnSettingsSave;
    public static event Action OnSettingsLoad;

    public static void LoadSettings()
    {
        OnSettingsSave?.Invoke();
    }

    public static void SaveSettings()
    {
        OnSettingsLoad?.Invoke();
    }
}