using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// Contains all Data needed for the CameraController.
/// </summary>
[CreateAssetMenu(menuName = "LCS/Camera/Camera Data")]
public class CameraData : ScriptableObject
{
    // BASE DATA //

    [Header("View")]
    public ViewType CameraViewType;
    public FadeType FadeType;
    [HorizontalLine(2, EColor.Gray)]

    // TRACKING DATA //

    [Header("Tracking")]
    public bool TrackTarget;                // Tracks the target of camera
    [EnableIf("TrackTarget")]
    public bool FocusOnPOI;                 // Will subtly track onto POI
    public bool AllowLookahead;             // Lets player use input to change look direction
    [HorizontalLine(2, EColor.Gray)]

    // AVOIDANCE //

    [Header("Avoidance")]
    public bool AvoidObstacles;
    [HorizontalLine(2, EColor.Gray)]

    // PATH DATA //

    [ShowIf("CameraViewType", ViewType.VIEW_PATH)]
    [Header("Pathing")]
    public Vector3 Path;            // Points the camera will follow
}