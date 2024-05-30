using LCS.Data;
using UnityEngine;

namespace LCS
{
    namespace GUI
    {
        public class SettingsGUI : MonoBehaviour
        {
            [SerializeField] private EmulatorSettingsGUI emulatorGUI;

            private SettingsData currentSettings;
            private SettingsDataFinder settingsFinder;

            #region Setup

            private void OnEnable()
            {
                settingsFinder = new SettingsDataFinder();
                currentSettings = settingsFinder.LoadSettings();

                // EMULATOR
                emulatorGUI.gameObject.SetActive(true);
                emulatorGUI.Construct(currentSettings);

                // VIDEO

                // AUDIO
            }

            private void OnDisable()
            {
                // needs to be additive rather than individual
                currentSettings = emulatorGUI.FinishEdit();

                // Save & Load Changes
                settingsFinder.SaveSettings(currentSettings);
                SettingsDataHandler.LoadSettings();
            }

            #endregion
        }
    }
}