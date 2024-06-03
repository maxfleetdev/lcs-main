using UnityEngine;

[CreateAssetMenu(menuName = "LCS/Camera/Camera Data")]
public class CameraData : ScriptableObject
{
    public CameraType CamType;
}

public enum CameraType
{
    CAM_DYNAMIC,
    CAM_STATIC,
    CAM_FOLLOW
}