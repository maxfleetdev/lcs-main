using System.Collections.Generic;
using UnityEngine;

public static class DifficultyCache
{
    private static string difficultyLocation = "Difficulty";
    private static Object[] difficulties;
    private static List<DifficultyData> cachedTypes = new List<DifficultyData>();

    public static void CacheDifficulty()
    {
        difficulties = Resources.LoadAll(difficultyLocation, typeof(DifficultyData));
        foreach (DifficultyData i in difficulties)
        {
            cachedTypes.Add(i);
        }
        Debugger.LogConsole("Cached all Difficulties | Resources/Difficulty", 0);
    }

    public static DifficultyData RetrieveDifficulty(DifficultyType type)
    {
        if (cachedTypes.Count == 0)
        {
            CacheDifficulty();
        }
        foreach (DifficultyData i in cachedTypes)
        {
            if (i.Type == type)
            {
                return i;
            }
        }
        Debugger.LogConsole($"No difficulty exists of type {type}", 0);
        return null;
    }
}