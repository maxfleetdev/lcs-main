using UnityEngine;
using UnityEngine.UI;

namespace LCS
{
    namespace GUI
    {
        public class EmulatorSettingsGUI : MonoBehaviour
        {
            [Header("Toggles")]
            [SerializeField] private Toggle crtToggle;
            [SerializeField] private Toggle vertexToggle;
            [SerializeField] private Toggle warpingToggle;
            [SerializeField] private Toggle filmToggle;

            [Header("Sliders")]
            [SerializeField] private Slider ditherSlider;
            [SerializeField] private Slider colorSlider;
            [SerializeField] private Slider resolutionSlider;

            private SettingsData settingsData = null;

            // Loads all the values from SettingsData to GUI elements
            public void Construct(SettingsData data)
            {
                settingsData = data;

                // toggles
                crtToggle.isOn                  = settingsData.CRTEffect;
                vertexToggle.isOn               = settingsData.VertexWobble;
                warpingToggle.isOn              = settingsData.TextureWarping;
                filmToggle.isOn                 = settingsData.FilmGrain;

                // sliders
                ditherSlider.value              = settingsData.DitherStrength * 100;
                colorSlider.value               = settingsData.ColorRange;
                resolutionSlider.value          = settingsData.ResolutionScale;
            }

            // Called when the saved data is required to be applied
            public SettingsData FinishEdit()
            {
                // booleans
                settingsData.CRTEffect          = crtToggle.isOn;
                settingsData.VertexWobble       = vertexToggle.isOn;
                settingsData.TextureWarping     = warpingToggle.isOn;
                settingsData.FilmGrain          = filmToggle.isOn;

                // floats
                settingsData.DitherStrength     = ditherSlider.value / 100;
                settingsData.ColorRange         = (int)colorSlider.value;
                settingsData.ResolutionScale    = (int)resolutionSlider.value;

                return settingsData;
            }
        }
    }
}