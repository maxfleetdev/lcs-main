using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class Command
{
    public class CommandData
    {
        public string Name { get; set; }
        public List<object> Values { get; set; }
    }

    private static CommandData ParseCommand(string input)
    {
        string[] parts = input.Split(' ');
        string commandName = parts[0];
        List<object> values = new List<object>();

        for (int i = 1; i < parts.Length; i++)
        {
            if (int.TryParse(parts[i], out int value))
            {
                values.Add(value);
            }
            else
            {
                values.Add(parts[i]);
            }
        }

        return new CommandData { Name = commandName, Values = values };
    }


    public static void ExecuteCommand(string input)
    {
        CommandData command = ParseCommand(input);
        List<object> values = command.Values;
        Debug.Log($">{input}");

        switch (command.Name.ToLower())
        {
            case "give":
                if (values.Count >= 2 && values[0] is int id && values[1] is int q)
                {
                    ItemData data = ItemCache.RetrieveItem(id);
                    if (data != null)
                        data.Pickup(q);
                }
                break;

            case "exit":
                if (values.Count >= 1 && values[0] is int exitCode && exitCode == 1)
                {
                    Application.Quit();
                    Debug.Log("Closing Application...");
                }
                break;

            case "save":
                if (values.Count >= 1 && values[0] is int saveSlot)
                {
                    GameDataHandler.SaveData(saveSlot);
                }
                break;

            case "load":
                if (values.Count >= 1 && values[0] is int loadSlot)
                {
                    GameDataHandler.LoadData(loadSlot);
                }
                break;

            case "delete":
                if (values.Count >= 1 && values[0] is int deleteSlot)
                {
                    GameDataHandler.DeleteData(deleteSlot);
                }
                break;

            case "gui":
                if (values.Count >= 1 && values[0] is int toggleValue)
                {
                    bool toggle = toggleValue > 0;
                    if (toggle) GUIHandler.ShowGUI(GUIType.GUI_SAVE_GAME);
                    else GUIHandler.HideGUI();
                }
                break;

            case "bootstrap":
                SceneManager.LoadScene(0, LoadSceneMode.Single);
                break;

            case "s_save":
                SettingsDataHandler.SaveSettings();
                break;

            case "s_load":
                SettingsDataHandler.LoadSettings();
                break;

            case "settings":
                if (values.Count >= 1 && values[0] is int settingsToggleValue && settingsToggleValue > 0)
                {
                    GUIHandler.ShowGUI(GUIType.GUI_SETTINGS);
                }
                break;

            case "scene":
                if (values.Count >= 1 && values[0] is string sceneName)
                {
                    SceneHandler.ChangeScene(sceneName);
                }
                break;

            default:
                Debug.Log($"Unknown Command: {command.Name.ToLower()}");
                break;
        }
    }
}