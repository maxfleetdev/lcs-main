using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[RequireComponent(typeof(Camera))]
public class EmulatorVisualManager : MonoBehaviour, ISettingsObject
{
    [SerializeField] private PSFXCameraLCS PSX;
    [SerializeField] private PostProcessVolume postProcessor;

    // Settings Data
    private SettingsData cameraSettings;

    // PSX VFX
    private bool vertexWobble;
    private bool textureWarping;
    private float ditherStrength;
    private int colorRange;
    private int resolutionScale;

    // Post Processing Volumes
    private bool crtEffect;
    private bool filmGrain;
    private RLProBleed bleedVolume;
    private RLProNoise noiseVolume;

    #region Startup

    private void Start()
    {
        // PSX SETUP //
        if (PSX == null)
        {
            Debugger.LogConsole("No PSX Emulator", 2, this);
            this.enabled = false;
            return;
        }
        PSX.enabled = true;

        // POST-PROCESSING SETUP //
        if (postProcessor == null)
        {
            Debugger.LogConsole("No CRT Emulator", 2, this);
            this.enabled = false;
            return;
        }
        postProcessor.profile.TryGetSettings(out bleedVolume);
        postProcessor.profile.TryGetSettings(out noiseVolume);
    }

    #endregion

    #region Save/Load Settings

    public void SaveSetting(SettingsData settings) { }      // does nothing atm

    public void LoadSetting(SettingsData settings)
    {
        cameraSettings = settings;

        this.crtEffect = cameraSettings.CRTEffect;
        this.filmGrain = cameraSettings.FilmGrain;

        this.vertexWobble = cameraSettings.VertexWobble;
        this.textureWarping = cameraSettings.TextureWarping;
        this.ditherStrength = cameraSettings.DitherStrength;
        this.colorRange = cameraSettings.ColorRange;
        this.resolutionScale = cameraSettings.ResolutionScale;
        
        ApplySettings();
    }

    #endregion

    #region Emulator Logic

    private void ApplySettings()
    {
        // Apply to PSFX VFX
        PSX.AffineStrength      = textureWarping ? 1.0f : 0.0f;
        PSX.VertexPrecision     = vertexWobble ? 5 : 0;
        PSX.ColorDepth          = colorRange;
        PSX.EnableColorDepth    = ditherStrength == 0 ? false : true;          // only enable if dithering is on
        PSX.DitheringStrength   = Mathf.Clamp(ditherStrength, 0, 0.1f);        // ranges from 0->10
        PSX.Resolution          = Mathf.Clamp(resolutionScale, 1, 8);

        // Apply to Post Processor
        bleedVolume.active      = crtEffect;
        noiseVolume.active      = filmGrain;
    }

    #endregion
}