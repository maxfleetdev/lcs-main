using UnityEngine;

[CreateAssetMenu(menuName = "LCS/Gameplay/Difficulty")]
public class DifficultyData : ScriptableObject
{
    public string DifficultyName;
    public DifficultyType Type;
    [Header("Difficulty Stats")]
    [Tooltip("Lower = Less damage taken | Higher = More damage taken")]
    public float TakeDamageMultiplier;
    [Tooltip("Lower = Less damage given | Higher = More damage given")]
    public float GiveDamageMultiplier;

    public void ChangeDifficulty()
    {
        DifficultyHandler.ChangeDifficulty(this.Type);
    }
}

public enum DifficultyType
{
    DIFFICULTY_EASY,
    DIFFICULTY_MEDIUM,
    DIFFICULTY_HARD,
    DIFFICULTY_INSANE,
}