using LCS.Data;
using System;
using System.Collections.Generic;

/// <summary>
/// Main (Data Exposed) Handler for saving and loading GameData files. Events are invoked to GameDataManager.
/// </summary>
public static class GameDataHandler
{
    // Subscribed by GameDataManager ONLY
    // Invoked by this class only
    public static event Action<int> SaveScene;
    public static event Action SaveNewScene;
    public static event Action<int> LoadScene;
    public static event Action<int> DeleteSave;

    #region Public Events

    /// <summary>
    /// Saves current scene/level to the assigned Data Index. Overwrites previous
    /// </summary>
    /// <param name="index"></param>
    public static void SaveData(int index)
    {
        SaveScene?.Invoke(index);
    }

    /// <summary>
    /// Creates a new Data folder using an unused Data Index
    /// </summary>
    public static void SaveNewData()
    {
        SaveNewScene?.Invoke();
    }

    /// <summary>
    /// Loads all GameData to IDataPersistence objects in scene
    /// </summary>
    /// <param name="index"></param>
    public static void LoadData(int index)
    {
        LoadScene?.Invoke(index);
    }

    /// <summary>
    /// Deletes the GameData file from the Disk
    /// </summary>
    /// <param name="index"></param>
    public static void DeleteData(int index)
    {
        DeleteSave?.Invoke(index);
    }

    /// <summary>
    /// Loads all GameData found on disk (use sparingly)
    /// </summary>
    /// <returns></returns>
    public static List<GameData> LoadAllFromDisk()
    {
        GameDataFinder writer = new GameDataFinder();
        return writer.LoadAllData();
    }

    #endregion
}