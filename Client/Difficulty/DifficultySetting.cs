using UnityEngine;

public enum DifficultyType
{
    Easy,
    Normal,
    Hard,
    Insane
}

[CreateAssetMenu(menuName = "LCS/Difficulty Setting")]
public class DifficultySetting : ScriptableObject
{
    [Tooltip("The multiplier when the player is attacked (High = More Damage)")]
    [SerializeField] private float damageTakeMultiplier;

    [Tooltip("The multiplier when the player attacks (High = More Damage)")]
    [SerializeField] private float damageHitMultiplier;

    [SerializeField] private DifficultyType difficultyType;     // not sure if needed
}