using UnityEngine;

[CreateAssetMenu(menuName = "LCS/Gameplay/Movement")]
public class MoveData : ScriptableObject
{
    [Header("Move Config")]
    public float WalkSpeed = 3f;
    public float SprintSpeed = 5f;
    public int RotationSpeed = 120;
    public float StrafeSpeed = 1.5f;

    [Header("Physics Config")]
    public float Gravity;
}