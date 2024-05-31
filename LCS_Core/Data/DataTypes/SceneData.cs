using System.Collections.Generic;

[System.Serializable]
public class SceneData
{
    public List<string> ObjectsRemoved;
    public List<string> PuzzlesCompleted;
    public List<string> EnemiesKilled;

    public SceneData()
    {
        this.ObjectsRemoved = new List<string>();
        this.PuzzlesCompleted = new List<string>();
        this.EnemiesKilled = new List<string>();
    }
}