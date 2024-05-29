using UnityEngine;

public class SettingsData
{
    // VOLUME DATA
    public float MasterVolume;
    public float MusicVolume;
    public float SfxVolume;

    // VIDEO DATA
    public Vector2Int VideoResolution;
    public bool Vsync;
    public int FPSLock;

    // EMULATOR DATA
    public bool CRTEffect;
    public bool VertexWobble;
    public bool TextureWarping;
    public bool FilmGrain;

    // CONTROL DATA
    public bool AutoLock;

    // Default Values
    public SettingsData()
    {
        this.MasterVolume = 1.0f;
        this.MusicVolume = 1.0f;
        this.SfxVolume = 1.0f;
        this.FPSLock = 60;

        this.CRTEffect = true;
        this.VertexWobble = true;
        this.TextureWarping = true;
        this.FilmGrain = true;
        
        this.AutoLock = true;
    }
}