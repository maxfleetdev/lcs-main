using System.Collections.Generic;
using UnityEngine;

public class GameData
{
    // GAME DATA
    public string SaveLocation;             // Used by SavePoints to save the location name (eg. House, Hospital etc)
    public string SaveTime;                 // The time and date it was last saved
    public int DataIndex;                   // Used for the amount of saves, first save is 1, second is 2 etc
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

    // OBJECT DATA
    public List<string> ObjectsRemoved;
    public List<string> PuzzlesCompleted;
    public List<string> EnemiesKilled;

    public GameData()
    {
        this.SaveLocation = "NULL";
        this.SaveTime = System.DateTime.Now.ToString();
        this.DataIndex = 0;
        this.GameDifficulty = DifficultyType.DIFFICULTY_MEDIUM;

        this.PlayerPositon = Vector3.zero;
        this.PlayerRotation = 0;
        this.Inventory = new List<Slot>();

        this.ShotsFired = 0;
        this.ShotAccuracy = 0f;
        this.KillAmount = 0;
        this.TotalDistance = 0f;
        this.GameScore = 0;

        this.ObjectsRemoved = new List<string>();
        this.PuzzlesCompleted = new List<string>();
        this.EnemiesKilled = new List<string>();
    }
}