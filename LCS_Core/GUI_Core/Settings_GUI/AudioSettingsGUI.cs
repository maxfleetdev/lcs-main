using UnityEngine;
using UnityEngine.UI;

public class AudioSettingsGUI : MonoBehaviour, ISettingsObject
{
    [SerializeField] private Slider masterSlider;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;

    public void LoadSetting(SettingsData settings)
    {
        masterSlider.value = settings.MasterVolume * 100;
        musicSlider.value = settings.MusicVolume * 100;
        sfxSlider.value = settings.SfxVolume * 100;
    }

    public void SaveSetting(SettingsData settings)
    {
        settings.MasterVolume = masterSlider.value / 100;
        settings.MusicVolume = musicSlider.value / 100;
        settings.SfxVolume = sfxSlider.value / 100;
    }
}