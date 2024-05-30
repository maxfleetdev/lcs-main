using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace LCS
{
    namespace Data
    {
        public class SettingsDataManager : MonoBehaviour
        {
            private List<ISettingsObject> settingObjects = new List<ISettingsObject>();

            #region Startup

            private void Awake()
            {
                SettingsDataHandler.OnSettingsSave += SaveSettingsToDisk;
                SettingsDataHandler.OnSettingsLoad += LoadSettingsFromDisk;
            }

            private void OnDisable()
            {
                SettingsDataHandler.OnSettingsSave -= SaveSettingsToDisk;
                SettingsDataHandler.OnSettingsLoad -= LoadSettingsFromDisk;
            }

            #endregion

            #region Save/Load Events

            private void SaveSettingsToDisk()
            {
                SettingsDataFinder writer = new SettingsDataFinder();
                SettingsData data = SaveSettings();
                writer.SaveSettings(data);
                SettingsDataHandler.SettingsSaved();
            }

            private void LoadSettingsFromDisk()
            {
                SettingsDataFinder loader = new SettingsDataFinder();
                SettingsData data = loader.LoadSettings();
                LoadSettings(data);
            }

            #endregion

            #region Get/Inject Events

            private void CacheObjects()
            {
                settingObjects.Clear();
                IEnumerable<ISettingsObject> persistence_objects = FindObjectsOfType<MonoBehaviour>().
                    OfType<ISettingsObject>();
                foreach (ISettingsObject mono in persistence_objects)
                {
                    print(persistence_objects.Count());
                    settingObjects.Add(mono);
                }
            }

            private SettingsData SaveSettings()
            {
                SettingsData data = new SettingsData();
                CacheObjects();
                foreach (ISettingsObject obj in settingObjects)
                {
                    obj.SaveSetting(data);
                }
                return data;
            }

            private void LoadSettings(SettingsData settings)
            {
                if (settings == null)
                {
                    SaveSettingsToDisk();
                    LoadSettingsFromDisk();
                    return;
                }
                CacheObjects();
                foreach (ISettingsObject obj in settingObjects)
                {
                    obj.LoadSetting(settings);
                }
            }

            #endregion
        }
    }
}