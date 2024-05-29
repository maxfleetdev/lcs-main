using System;

public static class DifficultyHandler
{
    public static event Action<DifficultyType> DifficultyType;

    public static void ChangeDifficulty(DifficultyType type)
    {
        DifficultyType?.Invoke(type);
    }
}