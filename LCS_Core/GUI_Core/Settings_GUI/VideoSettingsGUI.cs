using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

namespace LCS
{
    namespace GUI
    {
        public class VideoSettingsGUI : MonoBehaviour, ISettingsObject
        {
            [SerializeField] private TMP_Dropdown resolutionDropdown;
            [SerializeField] private Toggle vsyncToggle;

            // Resolutions
            private Resolution[] resolutions;
            private List<Resolution> filteredResolutions = new List<Resolution>();
            private int currentResolutionIndex;
            private RefreshRate currentRefreshRate;

            // Settings
            private Vector2Int currentResolution;

            public void LoadSetting(SettingsData settings)
            {
                // Resolution
                currentResolution = settings.VideoResolution;
                GenerateResolutions();

                // vSync
                vsyncToggle.isOn = settings.Vsync;
                // FPS Lock
            }

            public void SaveSetting(SettingsData settings)
            {
                // Resolution
                this.currentResolution.x = filteredResolutions[currentResolutionIndex].width;
                this.currentResolution.y = filteredResolutions[currentResolutionIndex].height;
                settings.VideoResolution = currentResolution;

                // vSync
                settings.Vsync = vsyncToggle.isOn;

                // FPS Lock
            }

            #region Resolution Logic

            private void GenerateResolutions()
            {
                // Flush
                filteredResolutions.Clear();

                currentRefreshRate = Screen.currentResolution.refreshRateRatio;
                resolutions = Screen.resolutions;
                resolutionDropdown.ClearOptions();

                for (int i = 0; i < resolutions.Length; i++)
                {
                    if (resolutions[i].refreshRateRatio.value == currentRefreshRate.value)
                    {
                        filteredResolutions.Add(resolutions[i]);
                    }
                }

                List<string> options = new List<string>();
                for (int i = 0; i < filteredResolutions.Count; i++)
                {
                    string resolution_option = $"{filteredResolutions[i].width}x{filteredResolutions[i].height}";
                    options.Add(resolution_option);
                    if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
                    {
                        currentResolutionIndex = i;
                    }
                }

                resolutionDropdown.AddOptions(options);
                resolutionDropdown.value = currentResolutionIndex;
                resolutionDropdown.RefreshShownValue();
            }

            public void SetResolution(int index)
            {
                Resolution resolution = filteredResolutions[index];
                Screen.SetResolution(resolution.width, resolution.height, true);
                currentResolutionIndex = index;
            }

            #endregion
        }
    }
}