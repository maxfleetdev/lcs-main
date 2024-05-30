using UnityEngine;

namespace LCS
{
    namespace GUI
    {
        public class SettingsGUI : MonoBehaviour, IGUIObject
        {
            [SerializeField] private GameObject guiElement;

            public void EnableGUI()
            {
                guiElement.SetActive(true);

                SettingsDataHandler.LoadSettings();
            }

            public void DisableGUI()
            {
                // Waits for settings to save before disabling GUI
                SettingsDataHandler.OnSettingsSaveComplete += SettingsSaved;
                SettingsDataHandler.SaveSettings();
                SettingsDataHandler.LoadSettings();
            }

            private void SettingsSaved()
            {
                SettingsDataHandler.OnSettingsSaveComplete -= SettingsSaved;
                guiElement.SetActive(false);
            }
        }
    }
}