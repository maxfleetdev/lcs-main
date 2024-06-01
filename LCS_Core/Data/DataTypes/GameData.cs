using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    // GAME DATA
    public string SaveLocation;
    public string SaveTime;
    public int DataIndex;
    public int SaveAmount;
    public DifficultyType GameDifficulty;

    // PLAYER DATA
    public Vector3 PlayerPositon;
    public int PlayerRotation;
    public List<Slot> Inventory;

    // GAME STATS
    public int ShotsFired;
    public float ShotAccuracy;
    public int KillAmount;
    public float TotalDistance;
    public int GameScore;

    // SCENE DATA
    public string CurrentScene;
    public SerializableDictionary<string, SceneData> SceneData;

    public GameData()
    {
        this.SaveLocation = "NULL";
        this.SaveTime = System.DateTime.Now.ToString();
        this.DataIndex = 0;
        this.SaveAmount = 0;
        this.GameDifficulty = DifficultyType.DIFFICULTY_MEDIUM;

        this.PlayerPositon = Vector3.zero;
        this.PlayerRotation = 0;
        this.Inventory = new List<Slot>();

        this.ShotsFired = 0;
        this.ShotAccuracy = 0f;
        this.KillAmount = 0;
        this.TotalDistance = 0f;
        this.GameScore = 0;

        this.CurrentScene = string.Empty;
        this.SceneData = new SerializableDictionary<string, SceneData>();
    }
}