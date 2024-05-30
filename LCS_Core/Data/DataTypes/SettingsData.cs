using UnityEngine;

public class SettingsData
{
    // VOLUME DATA
    public float MasterVolume;
    public float MusicVolume;
    public float SfxVolume;

    // VIDEO DATA
    public Vector2Int VideoResolution;
    public int ResolutionScale;
    public bool Vsync;
    public int FPSLock;

    // EMULATOR DATA
    public bool CRTEffect;
    public bool VertexWobble;
    public bool TextureWarping;
    public bool FilmGrain;
    public float DitherStrength;
    public int ColorRange;

    // CONTROL DATA
    public bool AutoLock;

    // Default Values
    public SettingsData()
    {
        this.MasterVolume = 1.0f;
        this.MusicVolume = 1.0f;
        this.SfxVolume = 1.0f;
        
        this.VideoResolution = new Vector2Int(1920, 1080);
        this.ResolutionScale = 2;
        this.Vsync = true;
        this.FPSLock = 60;

        this.CRTEffect = true;
        this.VertexWobble = true;
        this.TextureWarping = true;
        this.FilmGrain = true;
        this.DitherStrength = 0.05f;
        this.ColorRange = 12;
        
        this.AutoLock = true;
    }
}