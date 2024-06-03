using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SceneData
{
    // OBJECT DATA
    public List<string> ObjectsRemoved;

    // LOGIC DATA
    public List<string> PuzzlesCompleted;

    // ENEMY DATA
    public List<string> EnemiesKilled;
    public SerializableDictionary<string, Vector3> EnemyPositions;

    public SceneData()
    {
        this.ObjectsRemoved = new List<string>();
        this.PuzzlesCompleted = new List<string>();

        this.EnemiesKilled = new List<string>();
        this.EnemyPositions = new SerializableDictionary<string, Vector3>();
    }
}