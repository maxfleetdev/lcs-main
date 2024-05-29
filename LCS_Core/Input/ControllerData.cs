using UnityEngine;

[CreateAssetMenu(menuName = "LCS/Controls/Controller Data")]
public class ControllerData : ScriptableObject
{
    [Header("Deadzones")]
    public float XSlowMoveDeadzone = 0.5f;
    public float XFastMoveDeadzone = 0.85f;
    [Space]
    public float YSlowMoveDeadzone = 0.5f;
    public float YFastMoveDeadzone = 0.85f;
}