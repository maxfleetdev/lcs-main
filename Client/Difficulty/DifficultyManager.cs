using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/*
 *  Holds a list of all difficulties.
 *  Player selects the one they wish to use
 *  Changes the damage taken, hit and puzzle sensitivity
 */
public class DifficultyManager : MonoBehaviour
{
    // Serialized //
    [SerializeField] private List<DifficultySetting> difficulties = new List<DifficultySetting>();

    // Class-only //
    private static DifficultySetting selectedDifficulty;
    private static DifficultyManager instance;

    // Public Properites //
    public static DifficultySetting SelectedDifficulty
    {
        get => selectedDifficulty;
        private set => selectedDifficulty = value;
    }
    public static DifficultyManager Instance
    {
        get => instance;
        private set => instance = value;
    }

    #region Startup/Shutdown

    private void OnEnable()
    {
        if (Instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDisable()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    #endregion

    public void ChangeDifficulty(DifficultySetting difficulty)
    {
        if (difficulties.Contains(difficulty))
        {
            selectedDifficulty = difficulty;
        }

        else
        {
            DebugSystem.Log("Difficulty doesn't exist!", LogType.Warn);
        }
    }
}