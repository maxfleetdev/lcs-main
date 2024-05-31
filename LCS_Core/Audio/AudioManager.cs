using UnityEngine;
using FMODUnity;
using FMOD.Studio;

public class AudioManager : MonoBehaviour, ISettingsObject
{
    // SettingsData
    private float masterVolume;
    private float musicVolume;
    private float sfxVolume;

    // FMOD
    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;

    #region Startup

    private void Awake()
    {
        masterBus = RuntimeManager.GetBus("bus:/Master Bus");
        musicBus = RuntimeManager.GetBus("bus:/Master Bus/Music");
        sfxBus = RuntimeManager.GetBus("bus:/Master Bus/SFX");
    }

    #endregion

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
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        sfxBus.setVolume(sfxVolume);
    }
}