using UnityEngine;
using FMODUnity;

public class AudioManager : MonoBehaviour, ISettingsObject
{
    private float masterVolume;
    private float musicVolume;
    private float sfxVolume;

    #region Load Settings

    public void LoadSetting(SettingsData settings)
    {
        masterVolume = settings.MasterVolume; 
        musicVolume = settings.MusicVolume;
        sfxVolume = settings.SfxVolume;

        ApplySettings();
    }

    public void SaveSetting(SettingsData settings) { }

    #endregion

    private void ApplySettings()
    {
        
    }
}