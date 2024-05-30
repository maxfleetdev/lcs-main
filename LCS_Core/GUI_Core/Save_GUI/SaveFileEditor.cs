using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SaveFileEditor : MonoBehaviour
{
    [Header("Script Fields")]
    [SerializeField] private GameObject saveFileButton;
    [SerializeField] private Transform contentLocation;
    [SerializeField] private NewGameDataSave newSaveData;
    [Header("Text Fields")]
    [SerializeField] private TextMeshProUGUI dataText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private TextMeshProUGUI difficultyText;
    [SerializeField] private TextMeshProUGUI savedText;
    [Header("GUI Fields")]
    [SerializeField] private GameObject confirmationScreen;
    [SerializeField] private GameObject saveScreen;
    [Space]
    [SerializeField] private EventSystem eventSystem;

    // Cached Objects
    private GameData selectedGameData = null;
    private List<GameData> loadedData = new List<GameData>();
    private List<GameObject> loadedObjects = new List<GameObject>();

    #region Startup

    private void OnEnable()
    {
        confirmationScreen.SetActive(false);
        saveScreen.SetActive(true);
        PopulateSaves();
        newSaveData.OnSubmit += NewSave;
    }

    private void OnDisable()
    {
        // Cleanup Cache
        if (loadedData.Count > 0)
        {
            foreach (GameObject obj in loadedObjects)
            {
                if (obj != null)
                {
                    obj.GetComponent<GameDataSave>().OnSelected -= GameDataSelected;
                    Destroy(obj);
                }
            }
            loadedObjects.Clear();
        }
        newSaveData.OnSubmit -= NewSave;
    }

    #endregion

    #region UI Logic

    private void PopulateSaves()
    {
        // Cleanup Cache
        if (loadedData != null)
        {
            foreach (GameObject obj in loadedObjects)
            {
                if (obj != null)
                {
                    obj.GetComponent<GameDataSave>().OnSelected -= GameDataSelected;
                    obj.GetComponent<GameDataSave>().OnSubmit += ConfirmOverwrite;
                    Destroy(obj);
                }
            }
            loadedObjects.Clear();
        }

        // Get all saved GameData 
        loadedData = GameDataHandler.LoadAllFromDisk();
        if (loadedData == null)
            return;
        foreach (GameData data in loadedData)
        {
            // Need a better way of creating objects
            GameObject button = Instantiate(saveFileButton, contentLocation);
            GameDataSave data_save = button.GetComponent<GameDataSave>();
            if (data_save == null)
                continue;
            data_save.Construct(data);
            data_save.OnSelected += GameDataSelected;
            data_save.OnSubmit += ConfirmOverwrite;

            // Cache Object
            loadedObjects.Add(button);
        }

        // Setup EventSystem
        if (loadedObjects.Count > 0)
        {
            eventSystem.firstSelectedGameObject = loadedObjects[0];
            GameDataSelected(loadedData[0].DataIndex);
        }
    }

    private void GameDataSelected(int index)
    {
        foreach (GameData data in loadedData)
        {
            if (data.DataIndex == index)
                selectedGameData = data;
        }
        timeText.text = selectedGameData.SaveTime;
        dataText.text = $"Data {selectedGameData.DataIndex}";
        difficultyText.text = DifficultyCache.RetrieveDifficulty(selectedGameData.GameDifficulty).DifficultyName;
        savedText.text = $"Save {selectedGameData.SaveAmount}";
    }

    #endregion

    #region Data Logic

    // Asks for user confirmation
    private void ConfirmOverwrite()
    {
        saveScreen.SetActive(false);
        confirmationScreen.SetActive(true);
    }

    // Recieves users choice
    public void OverwriteChoice(bool choice)
    {
        if (choice)
        {
            GameDataHandler.SaveData(selectedGameData.DataIndex);
            GUIHandler.HideGUI();
            return;
        }
        PopulateSaves();
        confirmationScreen.SetActive(false);
        saveScreen.SetActive(true);
    }

    private void NewSave()
    {
        GameDataHandler.SaveNewData();
        PopulateSaves();
    }

    #endregion
}