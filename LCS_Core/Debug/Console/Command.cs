using UnityEngine;
using UnityEngine.SceneManagement;

public static class Command
{
    public struct CommandData
    {
        public string Name { get; set; }
        public int[] Values { get; set; }
    }

    private static CommandData ParseCommand(string input)
    {
        string[] parts = input.Split(' ');
        string commandName = parts[0];
        int[] values = new int[parts.Length - 1];
        for (int i = 1; i < parts.Length; i++)
        {
            if (int.TryParse(parts[i], out int value))
            {
                values[i - 1] = value;
            }
            else
            {
                Debugger.LogConsole($"Invalid value: {parts[i]}", 0);
            }
        }

        return new CommandData { Name = commandName, Values = values };
    }

    public static void ExecuteCommand(string input)
    {
        CommandData command = ParseCommand(input);
        int[] value = command.Values;
        Debugger.LogConsole($">{input}", 0);
        switch (command.Name.ToLower())
        {
            case "give":
                int id = value[0];               // Item ID
                int q = value[1];                // Quantity
                ItemData data = ItemCache.RetrieveItem(id);
                if (data != null)
                    data.Pickup(q);
                break;

            case "exit":
                if (value[0] == 1)
                    Application.Quit();
                Debugger.LogConsole($"Closing Application...", 0);
                break;

            case "save":
                GameDataHandler.SaveData(value[0]);
                break;

            case "load":
                GameDataHandler.LoadData(value[0]);
                break;

            case "delete":
                GameDataHandler.DeleteData(value[0]);
                break;

            case "gui":
                bool toggle = value[0] > 0 ? true : false;
                if (toggle) GUIHandler.ShowGUI(GUIType.GUI_SAVE_GAME);
                else GUIHandler.HideGUI();
                break;

            case "bootstrap":
                SceneManager.LoadScene(0, LoadSceneMode.Single);
                break;

            default:
                Debugger.LogConsole($"Unknown Command: {command.Name.ToLower()}", 0);
                break;
        }
    }
}