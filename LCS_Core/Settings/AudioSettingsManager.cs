using UnityEngine;

public class AudioSettingsManager : MonoBehaviour, ISettingsObject
{
    private float masterVolume;
    private float sfxVolume;
    private float musicVolume;

    public void LoadSetting(SettingsData settings)
    {
        masterVolume = settings.MasterVolume;
        sfxVolume = settings.SfxVolume;
        musicVolume = settings.MusicVolume;
    }

    public void SaveSetting(SettingsData settings) { }

    private void ApplyAudioSettings()
    {
        // changes for each audio source
    }
}