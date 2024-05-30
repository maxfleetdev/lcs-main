using UnityEngine;
using UnityEngine.UI;

namespace LCS
{
    namespace GUI
    {
        public class EmulatorSettingsGUI : MonoBehaviour, ISettingsObject
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

            public void LoadSetting(SettingsData settings)
            {
                // toggles
                this.crtToggle.isOn      = settings.CRTEffect;
                this.vertexToggle.isOn   = settings.VertexWobble;
                this.warpingToggle.isOn  = settings.TextureWarping;
                this.filmToggle.isOn     = settings.FilmGrain;
                
                // sliders
                this.ditherSlider.value      = settings.DitherStrength * 100;
                this.colorSlider.value       = settings.ColorRange;
                this.resolutionSlider.value  = settings.ResolutionScale;

                Debugger.LogConsole("Loading EmulatorSettings", 0);
            }

            public void SaveSetting(SettingsData settings)
            {
                // booleans
                settings.CRTEffect      = crtToggle.isOn;
                settings.VertexWobble   = vertexToggle.isOn;
                settings.TextureWarping = warpingToggle.isOn;
                settings.FilmGrain      = filmToggle.isOn;

                // floats
                settings.DitherStrength     = ditherSlider.value / 100;
                settings.ColorRange         = (int)colorSlider.value;
                settings.ResolutionScale    = (int)resolutionSlider.value;

                Debugger.LogConsole("Saving EmulatorSettings", 0);
            }
        }
    }
}