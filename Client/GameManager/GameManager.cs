using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Serialised Vars //
    [Header("Difficulty Config")]
    [SerializeField] private List<DifficultySetting> difficulties;

    // Class-only //
    private static GameManager instance;
    private DifficultySetting selectedDifficulty;

    // Public Properties //
    public static GameManager Instance
    {
        get => instance;
        private set => instance = value;
    }
    public DifficultySetting SelectedDifficulty
    {
        get => selectedDifficulty;
        private set => selectedDifficulty = value;
    }

    #region Startup/Shutdown

    private void OnEnable()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        // Change (depends on if player is loading save)
        if (selectedDifficulty == null)
        {
            selectedDifficulty = difficulties[0];
        }
        
        DebugSystem.Log($"{selectedDifficulty} selected", LogType.Debug);
    }

    private void OnDisable()
    {
        if (instance == this)
        {
            instance = null;
        }
    }

    #endregion

    public void ChangeDifficulty(DifficultySetting type)
    {
        if (difficulties.Contains(type))
        {
            selectedDifficulty = type;
        }
        else
        {
            DebugSystem.Log($"{type} is not a valid difficulty choice", LogType.Warn);
        }
    }
}