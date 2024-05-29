using UnityEngine;

namespace LCS
{
    namespace Data
    {
        public class SettingsDataManager : MonoBehaviour
        {
            #region Startup

            private void Awake()
            {
                SettingsDataHandler.OnSettingsSave += SaveSettingsToDisk;
                SettingsDataHandler.OnSettingsSave += LoadSettingsFromDisk;
            }

            private void OnDisable()
            {
                SettingsDataHandler.OnSettingsSave -= SaveSettingsToDisk;
                SettingsDataHandler.OnSettingsSave -= LoadSettingsFromDisk;
            }

            #endregion

            #region Save/Load Events

            private void SaveSettingsToDisk()
            {
                SettingsDataFinder writer = new SettingsDataFinder();
                writer.SaveSettings(SaveSettings());
            }

            private void LoadSettingsFromDisk()
            {
                SettingsDataFinder loader = new SettingsDataFinder();
                LoadSettings(loader.LoadSettings());
            }

            #endregion

            // Gets all objects which adjust settings (music manager, volume, mixer etc)
            // returns settingsdata
            private SettingsData SaveSettings()
            {
                return new SettingsData();
            }

            // Loads all objects which adjust settings (music manager, volume, mixer etc)
            private void LoadSettings(SettingsData settings)
            {

            }
        }
    }
}