using UnityEngine;

public class VideoSettingsManager : MonoBehaviour, ISettingsObject
{
    private SettingsData gameSettings;

    #region Startup

    private void Start()
    {
        // Initial Load of settings
        SettingsDataHandler.LoadSettings();
    }

    #endregion

    #region Load/Save

    public void LoadSetting(SettingsData settings)
    {
        if (settings == null) return;
        gameSettings = settings;
        ApplySettings();
    }

    public void SaveSetting(SettingsData settings)
    {
        settings.VideoResolution = new Vector2Int(Screen.width, Screen.height);
        settings.Vsync = QualitySettings.vSyncCount > 0 ? true : false;
        settings.FPSLock = Application.targetFrameRate;
    }

    #endregion

    private void ApplySettings()
    {
        Debugger.LogConsole("Applying Video Settings", 0);

        // SCREEN RESOLUTION & WINDOW MODE
        int x = gameSettings.VideoResolution.x, y = gameSettings.VideoResolution.y;
        Screen.SetResolution(x, y, true);
        // VSYNC & FPS LOCK
        QualitySettings.vSyncCount = gameSettings.Vsync ? 1 : 0;
        Application.targetFrameRate = gameSettings.FPSLock;
    }
}