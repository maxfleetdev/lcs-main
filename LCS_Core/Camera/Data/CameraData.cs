using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

[CreateAssetMenu(menuName = "LCS/Camera/Camera Data")]
public class CameraData : ScriptableObject
{
    // BASE DATA //

    [Header("View")]
    public ViewType CameraViewType;
    [HorizontalLine(2, EColor.Gray)]

    // TRACKING DATA //

    [Header("Tracking")]
    public bool TrackTarget;                // Tracks the target of camera
    
    [EnableIf("TrackTarget")]
    public bool FocusOnPOI;                 // Will subtly track onto POI
    
    [HorizontalLine(2, EColor.Gray)]

    // SPLINE DATA //

    [ShowIf("CameraViewType", ViewType.VIEW_SPLINE)]
    [Header("Spline Data")]
    public List<Vector3> SplinePoints;      // Points the camera will follow
}